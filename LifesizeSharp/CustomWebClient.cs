using System.Net;

namespace LifesizeSharp
{
    public class CustomWebClient : WebClient
    {
        public CustomWebClient()
        {
            System.Net.ServicePointManager.Expect100Continue = false;
        }
    }
}
