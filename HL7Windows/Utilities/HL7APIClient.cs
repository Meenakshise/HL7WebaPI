using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HL7Windows.Utilities
{
    public static class HL7APIClient
    {

        public static T GetAsync<T>(Uri url, string query)
        {
            using (var client = new HttpClient())
            {
                var actionUri = url + "/" + query;
                var webResponse = client.GetAsync(actionUri).Result;
                var result = webResponse.Content.ReadAsStringAsync().Result;
                if (webResponse.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(result);
                }
            }
            return default(T);
        }

        public static T PostAsync<T>(Uri url, string query, string postData)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var data = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("", postData) });
                var actionUri = url;
                var webResponse = client.PostAsync(actionUri, data).Result;
                var result = webResponse.Content.ReadAsStringAsync().Result;
                if (webResponse.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(result);
                }

            }
            return default(T);
        }
    }
}

