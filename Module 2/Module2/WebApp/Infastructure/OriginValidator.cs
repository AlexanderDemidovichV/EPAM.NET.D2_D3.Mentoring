using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WebApp.Models;

namespace WebApp.Infastructure
{
    public class OriginValidator: AbstractValidator<RegisterModel>
    {
        public OriginValidator()
        {
            RuleFor(model => model.UserName).NotEmpty().Length(1, 50);
            RuleFor(model => model.Email).Length(0, 200).EmailAddress();
            RuleFor(model => model.Age).NotNull().GreaterThan(0);
            RuleFor(model => model.Password).NotEmpty().Length(1, 50);
            RuleFor(model => model.ConfirmPassword).NotEmpty().Length(1, 50).Custom((field, context) =>
            {
                if (((RegisterModel) context.InstanceToValidate).Password != field)
                    context.AddFailure("Password isn't equal to ConfirmPassword");
            });
        }
    }
}
