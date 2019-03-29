using FluentValidation.TestHelper;
using NUnit.Framework;
using WebApp.Infastructure;
using WebApp.Models;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Name_HasErrorWhenNull()
        {
            var model = new RegisterModel
            {
                UserName = "petya",
                Age = 19,
                Password = "zdarova",
                ConfirmPassword = "privet"
            };

            var validationResult = new OriginValidator().Validate(model);

            //new OriginValidator().s(t => t.Password, "zdarova");
            //new OriginValidator().ShouldHaveValidationErrorFor(t => t.ConfirmPassword, "privet");
        }

        //[Test]
        //public void Name_DoesNotHaveErrorWhenValid()
        //{
        //    new OriginValidator().ShouldNotHaveValidationErrorFor(t => t.Name, "Table 1");
        //}

        //[Test]
        //public void Name_HasErrorWhenTooLong()
        //{
        //    new OriginValidator().ShouldHaveValidationErrorFor(t => t.Name, new string('a', 51));
        //}
    }
}