using System;
using System.Net;
using Newtonsoft.Json;

namespace LifesizeSharp
{
    public class LifesizeIconClient
    {
        public LifesizeCamera Camera { get; private set; }
        public LifesizeGui Gui { get; private set; }

        private CustomWebClient _webClient;
        private string _username;
        private string _password;

        public string SessionId { get; set; }

        public bool Connected { get; private set; }

        public LifesizeIconClient(string url)
        {
            _webClient = new CustomWebClient
            {
                BaseAddress = url + "/rest/", 
                Headers = {
                    {"X-Requested-With", "XMLHttpRequest"},
                    {"X-Client", "c#-client"}
                },
                Proxy = new WebProxy("localhost:8888")
            };

            Camera = new LifesizeCamera(_webClient, this);
            Gui = new LifesizeGui(_webClient, this);
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
    }
}
