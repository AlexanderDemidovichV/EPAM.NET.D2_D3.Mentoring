using ConditionalValidation;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [Test]
        public void CustomValidator_PasswordNotEqualConfirmPassword_ModelIsNotValid()
        {
            var model = new RegisterModel2
            {
                UserName = "petya",
                Age = 19,
                Password = "zdarova",
                ConfirmPassword = "privet"
            };

            var validationResult = new CustomValidator2().Validate(model);

            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void CustomValidator_PasswordEqualConfirmPassword_ModelIsValid()
        {
            var model = new RegisterModel2
            {
                UserName = "ttttttt",
                Age = 55,
                Password = "zdarova",
                ConfirmPassword = "zdarova"
            };

            var validationResult = new CustomValidator2().Validate(model);

            Assert.IsTrue(validationResult.IsValid); 
        }

        public class CustomValidator2 : AbstractValidator<RegisterModel2>
        {
            public CustomValidator2()
            {
                AddRule("Password", model => model.Password == model.ConfirmPassword);
            }
        }

        public class RegisterModel2
        {
            public string UserName { get; set; }

            public string Email { get; set; }

            public int Age { get; set; }

            public string Password { get; set; }

            public string ConfirmPassword { get; set; }
        }
    }
}