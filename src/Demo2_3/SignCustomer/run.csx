#r "Microsoft.WindowsAzure.Storage"
#r "SendGrid"
#r "Twilio.Api"

using System.Net;
using Microsoft.WindowsAzure.Storage.Table;
using FluentValidation;
using FluentValidation.Results;
using SendGrid.Helpers.Mail;
using Twilio;

public static HttpResponseMessage Run(HttpRequestMessage req, ICollector<Customer> customersTable, TraceWriter log, out Mail emailMessage, out SMSMessage smsMessage)
{
    emailMessage = new Mail();
    smsMessage = new SMSMessage();
    dynamic data = req.Content.ReadAsAsync<object>().Result;

    var customer = new Customer()
    {
        PartitionKey = "Functions",
        RowKey = Guid.NewGuid().ToString(),
        Name = data?.name,
        Surname = data?.surname,
        Country = data?.country,
        Email = data?.email,
        BirthYear = data?.birthyear
    };

    // Validation
    CustomerValidator validator = new CustomerValidator();
    ValidationResult results = validator.Validate(customer);
    if (results.IsValid == false)
    {
        return req.CreateResponse(HttpStatusCode.BadRequest, "Validation failed: "+ results.Errors.Select(error => error.ErrorMessage).Aggregate((a, b) => a + "; " + b));
    }

    // Store in table
    customersTable.Add(customer);

    // Send email
    var personalization = new Personalization();
    personalization.AddTo(new Email(customer.Email));
    emailMessage.AddPersonalization(personalization);

    // Send SMS
    smsMessage.Body = $"New customer: {customer.Name} {customer.Surname}";

    return req.CreateResponse(HttpStatusCode.OK);
}

public class Customer : TableEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Country { get; set; }
    public string Email { get; set; }
    public int BirthYear { get; set; }
}

public class CustomerValidator: AbstractValidator<Customer> 
{
    public CustomerValidator() 
    {
        RuleFor(customer => customer.Name).NotEmpty();
        RuleFor(customer => customer.Surname).NotEmpty();
        RuleFor(customer => customer.Country).NotEmpty();
        RuleFor(customer => customer.Email).EmailAddress();
        RuleFor(customer => customer.BirthYear).GreaterThanOrEqualTo(1900).LessThanOrEqualTo(DateTime.Now.Year);
    }
}