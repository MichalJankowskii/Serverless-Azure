using System.Net;
using FluentValidation;
using FluentValidation.Results;

public static HttpResponseMessage Run(HttpRequestMessage req, TraceWriter log)
{
    dynamic data = req.Content.ReadAsAsync<object>().Result;

    var customer = new Customer
    {
        Name = data?.name,
        Surname = data?.surname,
        Country = data?.country,
        Email = data?.email,
        BirthYear = data?.birthyear
    };

    // Validation
    CustomerValidator validator = new CustomerValidator();
    ValidationResult result = validator.Validate(customer);

    ValidationResultResponse response = new ValidationResultResponse
    { 
        DataCorrect = result.IsValid,
        Errors = ""
    };

    if (result.IsValid == false)
    {
        response.Errors = "Validation failed: "+ result.Errors.Select(error => error.ErrorMessage).Aggregate((a, b) => a + "; " + b);
    }

    return req.CreateResponse(HttpStatusCode.OK, response);
}

public class Customer
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Country { get; set; }
    public string Email { get; set; }
    public int BirthYear { get; set; }
}

public class ValidationResultResponse
{
    public bool DataCorrect { get; set; }
    public string Errors { get; set; }
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
