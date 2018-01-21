namespace RegistrationApp.Internal
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Host;
    using Microsoft.WindowsAzure.Storage.Table;
    using Models;

    public static class SaveCustomer
    {
        [FunctionName("SaveCustomer")]
        public static async Task Run(
            [QueueTrigger("tostorecustomer", Connection = "registration2storage_STORAGE")] Customer customer,
            [Table("customers", Connection = "registration2storage_STORAGE")] CloudTable customersTable,
            TraceWriter log)
        {
            log.Info($"SaveCustomer function processed: {customer.Name} {customer.Surname}");
            customer.PartitionKey = "AzureTest";
            customer.RowKey = Guid.NewGuid().ToString();

            TableOperation insertOperation = TableOperation.Insert(customer);
            await customersTable.ExecuteAsync(insertOperation);
        }
    }
}
