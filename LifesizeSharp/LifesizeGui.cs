using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LifesizeSharp
{
    public class LifesizeGui
    {
        private readonly CustomWebClient _webClient;
        private readonly LifesizeIconClient _parent;

        public LifesizeGui(CustomWebClient webClient, LifesizeIconClient parent)
        {
            _webClient = webClient;
            _parent = parent;
        }

        private t LifesizePost<t>(string call, Dictionary<string, object> parameters)
        {
            var jsonRequest = new Request()
            {
                Call = call,
                Parameters = parameters
            };

            var webResponse = _webClient.UploadString("request/" + _parent.SessionId + "/" + call,
                JsonConvert.SerializeObject(jsonRequest));
            return JsonConvert.DeserializeObject<t>(webResponse);
        }

        //ASYNC: Gui_startPresentation
        public void StartPresentation()
        {
            var response = LifesizePost<LifesizeResponse>("Gui_startPresentation", null);

            if (response.ReturnValue != 0)
                throw new Exception("Return Value: " + response.ReturnValue);
        }

        //ASYNC: Gui_stopPresentation
        public void StopPresentation()
        {
            var response = LifesizePost<LifesizeResponse>("Gui_stopPresentation", null);

            if (response.ReturnValue != 0)
                throw new Exception("Return Value: " + response.ReturnValue);
        }

        //SYNC: Gui_getPresentationState
        public string GetPresentationState()
        {
            var response = LifesizePost<GetPresentationState>("Gui_getPresentationState", null);

            if (response.ReturnValue != 0)
                throw new Exception("Return Value: " + response.ReturnValue);

            return response.State;
        }
    }

}
