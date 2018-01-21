namespace RegistrationApp
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs.Host;
    using Models;

    public static class RequestStatusCheck
    {
        [FunctionName("RequestStatusCheck")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestMessage req,
            [Table("processingStatus", Connection = "registration2storage_STORAGE")]IQueryable<ProcessingStatus> processingStatusTable,
            TraceWriter log)
        {
            string guid = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => String.Compare(q.Key, "guid", StringComparison.OrdinalIgnoreCase) == 0)
                .Value;

            log.Info($"Status check : {guid}");

            ProcessingStatus status =
                processingStatusTable.Where(processingStatus => processingStatus.RowKey == guid).FirstOrDefault();

            if (status == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a correct guid on the query string");
            }

            switch (status.ProcessingState)
            {
                case "Accepted":
                    return req.CreateResponse(HttpStatusCode.OK);
                case "Received":
                    return new HttpResponseMessage(HttpStatusCode.Accepted)
                    {
                        Headers =
                        {
                            Location = new Uri(ConfigurationManager.AppSettings["RequestStatusCheckUrl"] + guid)
                        }
                    };
                case "Error":
                    return req.CreateErrorResponse(HttpStatusCode.BadRequest, status.Message);
                default:
                    return req.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
