using System.Net.Http;

namespace CTESign.Core
{
    public sealed class HttpClientSingleton
    {
        private static readonly Lazy<HttpClient> _instance = new Lazy<HttpClient>(CreateHttpClient);

        private static HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
            httpClient.DefaultRequestHeaders.Add("DNT", "1");
            httpClient.DefaultRequestHeaders.Add("Origin", "https://forms.office.com");
            httpClient.DefaultRequestHeaders.Add("Referer", "https://forms.office.com/Pages/ResponsePage.aspx?id=GiZxC19JqU6ZEdqES5QC70KidcecKAVBgJ8h0Z3gjkVUNFFGTFhRTDMwTEtJN0M2TjZZWVlFNUZaUS4u");
            httpClient.DefaultRequestHeaders.Add("Sec-Ch-Ua", "\"Not; A = Brand\";v=\"24\", \"Chromium\";v=\"128\"");
            return httpClient;
        }

        public static HttpClient GetHttpClient() => _instance.Value;
    }
}
