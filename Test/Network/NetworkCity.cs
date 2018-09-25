using System;

using System.Collections.Generic;
using Test.EntityFramework;

namespace Test.Network
{
    public partial class AppNetwork
    {
        public async System.Threading.Tasks.Task<IList<EFCity>> requestCities()
        {
            IList<EFCity> result = null;
            string jsonResponse = await this.GETAsync(APIEndPoint.Cities);

            result = this._marshaller.marshallCities(jsonResponse);

            return (result);
        }

        public async System.Threading.Tasks.Task<IList<EFRegion>> requestRegions()
        {
            IList<EFRegion> result = null;
            string jsonResponse = await this.POSTAsync(APIEndPoint.Regions);

            result = this._marshaller.marshallRegions(jsonResponse);

            return (result);
        }

        public async System.Threading.Tasks.Task<EFRegion> requestRegion(String regionName)
        {
            EFRegion region = null;
            IDictionary<string, string> body = new Dictionary<string, string>();

            body["province"] = regionName;

            string jsonResponse = await this.POSTAsync(APIEndPoint.Regions, body);
            region = this._marshaller.marshallRegion(jsonResponse);

            return (region);
        }
    }
}
