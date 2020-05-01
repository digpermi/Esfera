using System;
using Entities.Models;
using FluentValidation;

namespace Entities.Validators
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            this.RuleFor(x => x.Code).NotEmpty();

            this.RuleFor(x => x.Identification).NotEmpty().Length(1, 50);

            this.RuleFor(x => x.FirstName).NotEmpty().Length(1, 200);

            this.RuleFor(x => x.LastName).NotEmpty().Length(1, 200);

            this.RuleFor(x => x.PhoneNumber).Length(1, 50);

            this.RuleFor(x => x.PhoneNumber).Length(1, 50);

            this.RuleFor(x => x.Email).Length(1, 100).EmailAddress();

            this.RuleFor(x => x.Birthdate).GreaterThan(new DateTime(1870, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        }
    }
}
