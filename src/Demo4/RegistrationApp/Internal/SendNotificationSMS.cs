namespace RegistrationApp.Internal
{
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Host;
    using Models;
    using Twilio;

    public static class SendNotificationSMS
    {
        [FunctionName("SendNotificationSMS")]
        public static void Run(
            [QueueTrigger("tosendnotification", Connection = "registration2storage_STORAGE")] Customer customer,
            [TwilioSms(
                To = "+48123456789",
                From = "+48123456789",
                Body = "New customer {Name} {Surname}!")]
            out SMSMessage message,
            TraceWriter log)
        {
            log.Info($"SendNotificationSMS function processed: {customer.Name} {customer.Surname}");
            message = new SMSMessage();
        }
    }
}
