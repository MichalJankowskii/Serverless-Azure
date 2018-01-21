namespace RegistrationApp
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs.Host;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Microsoft.WindowsAzure.Storage.Table;
    using Models;
    using Newtonsoft.Json;

    public static class SignCustomer
    {
        [FunctionName("SignCustomer")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] Customer customer,
            [Table("processingStatus", Connection = "registration2storage_STORAGE")] CloudTable proccessingStatusTable,
            [Queue("requestreceived", Connection = "registration2storage_STORAGE")] CloudQueue requestReceivedQueue,
            TraceWriter log)
        {
            log.Info($"Data received: {customer.Name} - {customer.Surname}");

            var processingStatus = new ProcessingStatus
            {
                ProcessingState = ProcessingState.Received.ToString()
            };

            customer.RowKey = processingStatus.RowKey;

            TableOperation insertOperation = TableOperation.Insert(processingStatus);
            await proccessingStatusTable.ExecuteAsync(insertOperation);

            string serializeCustomer = JsonConvert.SerializeObject(customer);
            await requestReceivedQueue.AddMessageAsync(new CloudQueueMessage(serializeCustomer));

            return new HttpResponseMessage(HttpStatusCode.Accepted)
            {
                Headers =
                {
                    Location = new Uri(ConfigurationManager.AppSettings["RequestStatusCheckUrl"] + processingStatus.RowKey)
                }
            };
        }
    }
}