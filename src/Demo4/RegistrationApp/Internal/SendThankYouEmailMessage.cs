namespace RegistrationApp.Internal
{
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Host;
    using Models;
    using SendGrid.Helpers.Mail;

    public static class SendThankYouEmailMessage
    {
        [FunctionName("SendThankYouEmailMessage")]
        public static void Run(
            [QueueTrigger("tosendemail", Connection = "registration2storage_STORAGE")] Customer customer,
            [SendGrid(
                To = "{Email}",
                Subject = "Thank you!",
                Text = "Hi {Name}, Thank you for registering!!!!",
                From = "test@azure.pl"
            )]
            out Mail message,
            TraceWriter log)
        {
            log.Info($"SendThankYouEmailMessage function processed: {customer.Name} {customer.Surname}");
            message = new Mail();
        }
    }
}
