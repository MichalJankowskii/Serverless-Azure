namespace UrlShortener
{
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Google.Apis.Services;
    using Google.Apis.Urlshortener.v1;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs.Host;

    public static class Url_Shortener
    {
        [FunctionName("Url_Shortener")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            // parse query parameter
            string url = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "url", true) == 0)
                .Value;

            // Get request body
            dynamic data = await req.Content.ReadAsAsync<object>();

            // Set name to query string or body data
            url = url ?? data?.url;

            log.Info($"Url: {url}");

            UrlshortenerService service = new UrlshortenerService(new BaseClientService.Initializer()
            {
                ApiKey = ConfigurationManager.AppSettings["GoogleAPIKey"],
                ApplicationName = "URL shortener",
            });
            var m = new Google.Apis.Urlshortener.v1.Data.Url {LongUrl = url};

            return url == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a url on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, service.Url.Insert(m).Execute().Id);
        }
    }
}
