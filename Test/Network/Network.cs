using System;

using System.Net.Http;
using System.Collections;
using Newtonsoft.Json;

namespace Test.Network
{
    public enum APIEndPoint
    {
        JSONCall
    }

    public class AppNetwork 
    {
        private string _baseURL = "http://192.168.33.10/Test/index.php/";
        private HttpClient _client;

        public async System.Threading.Tasks.Task<ArrayList> GETAsync(APIEndPoint endPoint)
        {
            ArrayList result = null;
            HttpResponseMessage message = null;

            this._client = new HttpClient();

            try {

                var uri = this.constructURI(endPoint);
                message = await this._client.GetAsync(uri);
            }
            catch (Exception e) {

                Console.WriteLine(e.Message);
            }

            if (message == null || message.IsSuccessStatusCode) {

                var content = await message.Content.ReadAsStringAsync();

                try {

                    result = JsonConvert.DeserializeObject<ArrayList>(content);
                }
                catch (Exception e) {

                    Console.WriteLine("FAILED TO READ JSON: {0}", e.Message);
                    result = null;
                }
            }

            this._client.CancelPendingRequests();
            this._client = null;

            return result;
        }

        private string constructURI(APIEndPoint endPoint)
        {
            string result = null;

            switch(endPoint) {

                case APIEndPoint.JSONCall: { result = $"{this._baseURL}/json"; } break;
            }

            return (result);
        }
    }
}
