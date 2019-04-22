using System.ComponentModel.DataAnnotations;
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
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-api-key", apikey);
            var req = string.Format(URL, appid, query);
            var response = client.GetAsync(req).Result;
            return response.IsSuccessStatusCode ? 
                response.Content.ReadAsStringAsync().Result : response.ReasonPhrase;
        }

    }
}
