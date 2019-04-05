using ConditionalValidation;
using WebApp.Models;

namespace WebApp.Infastructure
{
    public class RegisterModelValidator: AbstractValidator<RegisterModel>
    {
        public RegisterModelValidator()
        {
            AddRule("Password", model => model.Password == model.ConfirmPassword);
            AddRule("Age", model => model.Age > 50 && model.Age != 22);
        }
    }
}
