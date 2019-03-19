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
        public void TestMethod1()
        {
            //var config = new MapperConfiguration(cfg => {

            //    cfg.CreateMap<Foo, Bar>().ForMember(destination => destination.ContactDetails,

            //        opts => opts.MapFrom(source => source.Contact));

            //});

            //var mapGenerator = new MappingGenerator();
            //var mapper = mapGenerator.Generate<Foo, Bar>();
            //mapper.ForMember(src => src.Contact, dest => dest.ContactDetails);

            //mapper.ForMember(src => src.Budget, dest => dest.Stock);

            //var foo = new Foo
            //{
            //    Contact = "Prop1",
            //    Budget = 42
            //};
            //var bar = mapper.Map(foo);


        }

     

        [Test]
        public void Test3()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo2, Bar2>();
            mapper.AddMapping(src => src.Prop1, dest => dest.Prop1);
            mapper.AddMapping(src => src.Prop2, dest => dest.Prop2);
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
