using System;

using System.Net.Http;
using System.Collections;
using Newtonsoft.Json;

namespace Test
{
    public class Network 
    {
        private HttpClient _client;

        public async System.Threading.Tasks.Task<ArrayList> GETAsync(String uri)
        {
            ArrayList result = null;
            HttpResponseMessage message = null;

            this._client = new HttpClient();

            try {

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
    }
}
