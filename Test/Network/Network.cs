using System;

using System.Net.Http;
using System.Collections;
using System.Collections.Generic;
using Test.EntityFramework;

namespace Test.Network
{
    public enum APIEndPoint
    {
        Cities,
        Regions
    }

    public partial class AppNetwork
    {
        private AppNetMarshaller _marshaller;
        private HttpClient _client;
        private string _baseURL = "http://192.168.33.10/Test/index.php";

        public AppNetwork()
        {
            this._client = new HttpClient();
            this._marshaller = new AppNetMarshaller();
        }

        public void cancelPending()
        {
            this._client.CancelPendingRequests();
        }

        protected async System.Threading.Tasks.Task<string> GETAsync(APIEndPoint endPoint)
        {
            HttpResponseMessage message = null;
            string result = null;

            try {

                var uri = this.constructURI(endPoint);
                message = await this._client.GetAsync(uri);
            }
            catch (Exception e) {

                Console.WriteLine(e.Message);
            }

            if (message == null || message.IsSuccessStatusCode) {

                result = await message.Content.ReadAsStringAsync();
            }

            return result;
        }

        protected System.Threading.Tasks.Task<string> POSTAsync(APIEndPoint endPoint)
        {
            return (this.POSTAsync(endPoint, null));
        }

        protected async System.Threading.Tasks.Task<string> POSTAsync(APIEndPoint endPoint, IDictionary<string, string> body)
        {
            HttpResponseMessage message = null;
            string result = null;
            string uri = this.constructURI(endPoint);

            if (body == null)
            {
                body = new Dictionary<string, string>();
            }
            try {

                var postBody = new FormUrlEncodedContent(body);
                message = await this._client.PostAsync(uri, postBody);

                result = await message.Content.ReadAsStringAsync();
            }
            catch (Exception e) {

                Console.WriteLine("FAILED TO POST TO: {0}", e.Message);
            }

            return (result);
        }

        private string constructURI(APIEndPoint endPoint)
        {
            string result = null;

            switch(endPoint) {

                case APIEndPoint.Cities: { result = $"{this._baseURL}/json"; } break;
                case APIEndPoint.Regions: { result = $"{this._baseURL}/regions"; } break;
            }

            return (result);
        }
    }
}
