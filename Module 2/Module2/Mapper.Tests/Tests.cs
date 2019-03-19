using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//using AutoMapper;
using NUnit.Framework;

namespace Mapper.Tests
{
    class Tests
    {
        public class Foo
        {
            public string Contact { get; set; }
            public int Budget { get; set; }
        }

        public class Bar
        {
            public string ContactDetails { get; set; }
            public int Stock { get; set; }
        }

     

        [Test]
        public void Test()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo2, Bar2>();
            mapper.ForMember(src => src.Prop1, dest => dest.Prop1);
            var foo = new Foo2
            {
                Prop1 = "Prop1",
                Prop2 = 2605
            };
            var bar = mapper.Map(foo);
        }
        public class Foo2
        {
            public string Prop1 { get; set; }
            public int Prop2 { get; set; }
        }
        public class Bar2
        {
            public string Prop1 { get; set; }
            public int Prop2 { get; set; }
        }
    }


}
