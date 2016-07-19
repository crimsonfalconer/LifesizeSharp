using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static System.String;

namespace LifesizeSharp
{
    public class LifesizeIconClient
    {
        public string SessionId { get; set; }
        public bool Connected { get; private set; }

        public LifesizeCamera Camera { get; private set; }
        public LifesizeGui Gui { get; private set; }

        private CustomWebClient _webClient;
        private string _username;
        private string _password;
        private string _baseUrl;
        private Task _longPoll;
        private bool _keepPolling;

        public LifesizeIconClient(string url)
        {
            _baseUrl = url;
            _webClient = NewWebClient(url + "/rest/");

            Camera = new LifesizeCamera(_webClient, this);
            Gui = new LifesizeGui(_webClient, this);
        }

        private CustomWebClient NewWebClient(string baseUrl)
        {
            return new CustomWebClient
            {
                BaseAddress = baseUrl,
                Headers = {
                    {"X-Requested-With", "XMLHttpRequest"},
                    {"X-Client", "c#-client"}
                },
                Proxy = new WebProxy("localhost:8888")
            };
        }

        private t LifesizePost<t>(string call, Dictionary<string, object> parameters)
        {
            var jsonRequest = new Request()
            {
                Call = call,
                Parameters = parameters
            };

            var webResponse = _webClient.UploadString("request/" + SessionId + "/" + call,
                JsonConvert.SerializeObject(jsonRequest));
            return JsonConvert.DeserializeObject<t>(webResponse);
        }

        public void Connect(string username, string password)
        {
            _username = username;
            _password = password;
            _webClient.Credentials = new NetworkCredential(username, password);

            LifesizeLoginResponse loginResponse;

            try
            {
                var webResponse = _webClient.DownloadString("new");
                loginResponse = JsonConvert.DeserializeObject<LifesizeLoginResponse>(webResponse);
            }
            catch (WebException e)
            {
                HttpWebResponse response = (System.Net.HttpWebResponse)e.Response;
                throw new Exception(response.StatusCode + " - " + response.StatusDescription, e);
            }

            if (loginResponse.ReturnValue == 0 && loginResponse.Session != null)
            {
                Connected = true;
                SessionId = loginResponse.Session;
            }
        }

        public void Listen(List<string> eventList)
        {
            var response = LifesizePost<LifesizeResponse>("listen", new Dictionary<string, object>
            {
                { "events", Join(",", eventList)}
            });
            Task.Run(() => StartPolling());

            if (response.ReturnValue != 0)
                throw new Exception("Return Value: " + response.ReturnValue);
        }

        public async void StartPolling()
        {
            _keepPolling = true;
            do
            {
                var t = await LongPoll();
                ProcessEvents(t);
            } while (_keepPolling);
        }

        public void StopPolling()
        {
            _keepPolling = false;
        }

        public void ProcessEvents(string eventJson)
        {
            //[{"params": {"enabled": 0, "dev": 131072}, "call": "Camera_lockEnabledChanged"}]
            Debug.WriteLine("Processing Events!");
            var eventList = JsonConvert.DeserializeObject<List<RequestString>>(eventJson);
            foreach (var e in eventList)
            {
                switch (e.Call)
                {
                    case "Camera_lockEnabledChanged":
                        Camera.OnLockEnabledChanged(JsonConvert.DeserializeObject<EnableChanged>(e.Parameters.ToString()));
                        break;
                    case "Gui_presentationStateChanged":
                        Gui.OnPresentationStateChanged(JsonConvert.DeserializeObject<StateChanged>(e.Parameters.ToString()));
                        break;
                }
                Debug.WriteLine(e.Call);
            }
        }

        public async Task<string> LongPoll()
        {
            var result = await Task.Run(() =>
            {
                var client = NewWebClient(_webClient.BaseAddress);
                return client.DownloadString("/longpoll.rb?session=" + SessionId + "&timeout=30");
            });
            return result;
        }

    }
}
