using System;
using System.Collections.Generic;
using System.Text;

namespace ConditionalValidation
{
    class TestModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    class TestValidation : AbstractValidator<TestModel>
    {
        public TestValidation()
        {
            AddRule(m => m.Type != "reserved" && m.Name != "admin");
        }
    }
}
