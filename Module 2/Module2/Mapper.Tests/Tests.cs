using NUnit.Framework;

namespace Mapper.Tests
{
    class Tests
    {
        internal class Foo
        {
            public string Contact { get; set; }
            public int Budget { get; set; }
        }

        internal class Bar
        {
            public string ContactDetails { get; set; }
            public int Stock { get; set; }
        }

        private IMapper<Foo, Bar> _mapper;

        [SetUp]
        public void SetUp()
        {
            var mapGenerator = new MappingGenerator();
            _mapper = mapGenerator.Generate<Foo, Bar>();
        }

        [Test]
        public void Map_StringProp_StringPropValueMapped()
        {
            _mapper
                .ForMember(src => src.Contact, dest => dest.ContactDetails);
            var foo = new Foo
            {
                Contact = "00000000-0000-0000-0000-000000000000",
                Budget = 205
            };

            var bar = _mapper.Map(foo);

            Assert.AreNotEqual(foo.Budget, bar.Stock);
            Assert.AreEqual(foo.Contact, bar.ContactDetails);
        }

        [Test]
        public void Map_IntProp_IntPropValueMapped()
        {
            _mapper
                .ForMember(src => src.Budget, dest => dest.Stock);
            var foo = new Foo
            {
                Contact = "00000000-0000-0000-0000-000000000000",
                Budget = 3
            };

            var bar = _mapper.Map(foo);

            Assert.AreEqual(foo.Budget, bar.Stock);
            Assert.AreNotEqual(foo.Contact, bar.ContactDetails);
        }
    }


}
