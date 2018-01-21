namespace SubmitToEventGrid
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs.Host;
    
    public static class SubmitToEventGrid
    {
        [FunctionName("SubmitToEventGrid")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var events = new List<Event>
            {
                new Event()
                {
                    id = Guid.NewGuid().ToString(),
                    eventTime = DateTime.UtcNow,
                    eventType = "user",
                    subject = "add",
                    data = UserFactory.BuildUser()
                }
            };

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("aeg-sas-key", "659s7yzJL36Jq5SdJC7H4hSgX6lYoPG7Am2h8Gdt0+Y=");
            await httpClient.PostAsJsonAsync("https://progressiveneteventgrid.westus2-1.eventgrid.azure.net/api/events", events);

            return req.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}
