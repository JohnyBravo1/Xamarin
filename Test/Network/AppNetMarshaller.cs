using System;

using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Test.EntityFramework;

namespace Test.Network
{
    public class AppNetMarshaller
    {
        public EFCity marshallCity(string json)
        {
            EFCity result = null;

            try {

                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Converters.Add(new CityConverter());
                result = JsonConvert.DeserializeObject<EFCity>(json, settings);
            }
            catch (Exception e) {

                Console.WriteLine($"marshallCity failed with: {e.Message}");
            }

            return (result);
        }

        public IList<EFCity> marshallCities(string json)
        {
            IList<EFCity> result = null;

            try {

                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Converters.Add(new CityConverter());
                result = JsonConvert.DeserializeObject<IList<EFCity>>(json, settings);
            }
            catch (Exception e) {

                Console.WriteLine("marshallCities failed with error: {0}", e.Message);
            }

            return (result);
        }

        public IDictionary marshallDictionary(string jsonString)
        {
            IDictionary result = null;

            try {

                result = JsonConvert.DeserializeObject<IDictionary>(jsonString);
            }
            catch (Exception e)
            {

                Console.WriteLine("marshallList failed with error: {0}", e.Message);
            }

            return (result);
        }

        public ArrayList marshallList(string jsonString)
        {
            ArrayList result = null;

            try {

                result = JsonConvert.DeserializeObject<ArrayList>(jsonString);
            }
            catch (Exception e) {

                Console.WriteLine("marshallList failed with error: {0}", e.Message);
            }

            return (result);
        }

        public IList<EFRegion> marshallRegions(string json)
        {
            IList<EFRegion> result = null;

            try {

                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Converters.Add(new RegionsConverter());

                result = JsonConvert.DeserializeObject<IList<EFRegion>>(json, settings);
            }
            catch (Exception e) {

                Console.WriteLine("marshallRegions failed with error: {0}", e.Message);
            }

            return (result);
        }

        public EFRegion marshallRegion(string json)
        {
            EFRegion result = null;

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new RegionConverter());

            result = JsonConvert.DeserializeObject<EFRegion>(json, settings);

            return (result);
        }
    }

    class CityConverter : JsonConverter<EFCity>
    {
        public override EFCity ReadJson(JsonReader reader,
                                        Type objectType, 
                                        EFCity existingValue, 
                                        bool hasExistingValue, 
                                        JsonSerializer serializer)
        {
            JObject o = serializer.Deserialize<JObject>(reader);
            EFCity result = (existingValue == null ? new EFCity() : existingValue);

            string cityName = (string)o["name"];

            if (cityName != null) {

                result.Name = cityName;
            }

            return (result);
        }

        public override void WriteJson(JsonWriter writer, 
                                       EFCity value, 
                                       JsonSerializer serializer)
        {

        }
    }

    class RegionConverter : JsonConverter<EFRegion>
    {
        public override EFRegion ReadJson(JsonReader reader, 
                                          Type objectType, 
                                          EFRegion existingValue, 
                                          bool hasExistingValue, 
                                          JsonSerializer serializer)
        {
            EFRegion result = (existingValue == null ? new EFRegion() : existingValue);
            JObject o = serializer.Deserialize<JObject>(reader);

            //{{"province": [{"city": [{"name": "Pretoria"}, { "name": "Johannesburg"}, { "name": "Cullinan"}], "name": "Gauteng"}]}}
            string provinceName = (string)o["province"][0]["name"];

            result.Name = provinceName;

            EFCity city;

            foreach (JObject cityObject in o["province"][0]["city"])
            {
                city = new EFCity();

                city.Name = (string)cityObject["name"];
                result.AppendCity(city);
            }

            return (result);
        }

        public override void WriteJson(JsonWriter writer, 
                                       EFRegion value, 
                                       JsonSerializer serializer)
        {

        }
    }

    class RegionsConverter : JsonConverter<IList<EFRegion>>
    {
        public override IList<EFRegion> ReadJson(JsonReader reader, 
                                                 Type objectType, 
                                                 IList<EFRegion> existingValue, 
                                                 bool hasExistingValue, 
                                                 JsonSerializer serializer)
        {
            IList<EFRegion> result = (existingValue == null ? new List<EFRegion>() : existingValue);
            JObject o = serializer.Deserialize<JObject>(reader);

            JArray provinces = o["province"] as JArray;
            EFCity city;
            EFRegion region;

            foreach (JObject province in provinces)
            {
                region = new EFRegion();

                region.Name = (string)province["name"];

                foreach(JObject cityObject in province["city"])
                {
                    city = new EFCity();

                    city.Name = (string)cityObject["name"];
                    region.AppendCity(city);
                }

                result.Add(region);
            }

            return (result);
        }

        public override void WriteJson(JsonWriter writer, 
                                       IList<EFRegion> value, 
                                       JsonSerializer serializer)
        {

        }
    }
}
