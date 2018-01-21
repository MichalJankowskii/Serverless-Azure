namespace RegistrationApp.Validators
{
    using System;
    using FluentValidation;
    using Models;

    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            this.RuleFor(customer => customer.Name).NotEmpty();
            this.RuleFor(customer => customer.Surname).NotEmpty();
            this.RuleFor(customer => customer.Country).NotEmpty();
            this.RuleFor(customer => customer.Email).EmailAddress();
            this.RuleFor(customer => customer.BirthYear).GreaterThanOrEqualTo(1900).LessThanOrEqualTo(DateTime.Now.Year);
        }
    }
}
