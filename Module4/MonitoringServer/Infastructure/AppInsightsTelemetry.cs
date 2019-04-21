using System.Net.Http;
using System.Net.Http.Headers;

namespace MonitoringServer.Infastructure
{
    public static class AppInsightsTelemetry
    {
        private const string URL =
            "https://api.applicationinsights.io/v1/apps/{0}/query?{1}";

        public static string GetTelemetry(string appid, string apikey,
            string query)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-api-key", apikey);
            var req = string.Format(URL, appid, query);
            HttpResponseMessage response = client.GetAsync(req).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
            }
            return response.ReasonPhrase;
        }

    }
}
