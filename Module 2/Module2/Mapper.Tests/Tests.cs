using Mapper.Interfaces;
using NUnit.Framework;

namespace Mapper.Tests
{
    [TestFixture]
    public class MapperTest
    {
        private IMapper<Source, Destination> _mapper;

        [SetUp]
        public void Initialize()
        {
            var mapGenerator = new MappingGenerator();
            _mapper = mapGenerator.Generate<Source, Destination>();
        }

        [Test]
        public void GetNewDestinationInstanceTest()
        {
            var result = _mapper.Map(new Source());
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Destination>(result);
        }

        [Test]
        public void MapPropertiesWithSameNameTest()
        {
            var source = new Source { A = "one" };
            var result = _mapper.Map(source);
            Assert.IsNotNull(result?.A);
            Assert.AreEqual(source.A, result.A);
        }

        [Test]
        public void MapPropertiesWithDifferentNameTest()
        {
            var source = new Source { A = "one", B = "two", C = "three" };
            var result = _mapper.Map(source);
            Assert.AreEqual(source.A, result.A);
            Assert.AreEqual(source.B, result.B);
            Assert.IsNull(result.D);
            Assert.IsNotNull(source.C);
        }

        [Test]
        public void MapPropertiesWithComplexTypesTest()
        {
            var source = new Source { Transfer = new Transfer() };
            var result = _mapper.Map(source);
            Assert.IsNotNull(result.Transfer);
            Assert.AreEqual(result.Transfer.Info, source.Transfer.Info);
        }

        [Test]
        public void MapFieldsWithSameNameTest()
        {
            var source = new Source { E = "one" };
            var result = _mapper.Map(source);
            Assert.IsNotNull(result.E);
            Assert.AreEqual(source.E, result.E);
        }

        [Test]
        public void MapFieldsWithDifferentNameTest()
        {
            var source = new Source { E = "one", F = "two", G = "three" };
            var result = _mapper.Map(source);
            Assert.AreEqual(source.E, result.E);
            Assert.AreEqual(source.F, result.F);
            Assert.IsNull(result.H);
            Assert.IsNotNull(source.G);
        }
    }

    public class Source
    {
        public string A { get; set; }

        public string B { get; set; }

        public string C { get; set; }

        public Transfer Transfer { get; set; }

        public string E;

        public string F;

        public string G;
    }

    public class Destination
    {
        public string A { get; set; }

        public string B { get; set; }

        public string D { get; set; }

        public Transfer Transfer { get; set; }

        public string E;

        public string F;

        public string H;
    }

    public class Transfer
    {
        public string Info { get; set; }
        public Transfer()
        {
            Info = "Info";
        }
    }


}
