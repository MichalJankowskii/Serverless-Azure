namespace RegistrationApp.Internal
{
    using System.Linq;
    using System.Threading.Tasks;
    using FluentValidation.Results;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Host;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Microsoft.WindowsAzure.Storage.Table;
    using Models;
    using Newtonsoft.Json;
    using Validators;

    public static class RequestValidation
    {
        [FunctionName("RequestValidation")]
        public static async Task Run(
            [QueueTrigger("requestreceived", Connection = "registration2storage_STORAGE")]Customer customer,
            [Queue("requestaccepted", Connection = "registration2storage_STORAGE")] CloudQueue requestAcceptedQueue,
            [Table("processingStatus", Connection = "registration2storage_STORAGE")] CloudTable proccessingStatusTable,
            TraceWriter log)
        {
            log.Info($"RequestValidation function processed: {customer.RowKey} ({customer.Name} - {customer.Surname})");

            var validator = new CustomerValidator();
            ValidationResult results = validator.Validate(customer);

            var processingStatus = new ProcessingStatus(customer.RowKey)
            {
                ETag = "*"
            };

            if (results.IsValid)
            {
                await requestAcceptedQueue.AddMessageAsync(new CloudQueueMessage(JsonConvert.SerializeObject(customer)));
                processingStatus.ProcessingState = ProcessingState.Accepted.ToString();
            }
            else
            {
                processingStatus.ProcessingState = ProcessingState.Error.ToString();
                processingStatus.Message = "Validation failed: " +
                                           results.Errors.Select(error => error.ErrorMessage)
                                               .Aggregate((a, b) => a + "; " + b);
            }

            TableOperation updateOperation = TableOperation.Replace(processingStatus);
            await proccessingStatusTable.ExecuteAsync(updateOperation);
        }
    }
}
