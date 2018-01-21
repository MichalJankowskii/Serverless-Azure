namespace RegistrationApp.Models
{
    using System;
    using Microsoft.WindowsAzure.Storage.Table;
    public class ProcessingStatus : TableEntity
    {
        public ProcessingStatus() : this(null)
        {
        }

        public ProcessingStatus(string guid)
        {
            this.RowKey = guid ?? Guid.NewGuid().ToString();
            this.PartitionKey = "AzureTest";
        }

        public string ProcessingState { get; set; }

        public string Message { get; set; }
    }
}
