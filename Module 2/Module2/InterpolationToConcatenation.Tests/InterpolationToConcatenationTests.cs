using System.Collections.Generic;
using System.Linq;
using InterpolationToConcatenation.InterpolationReplacer;
using InterpolationToConcatenation.Models;
using NUnit.Framework;

namespace InterpolationToConcatenation.Tests
{
    [TestFixture]
    public class InterpolationToConcatenationTests
    {
        private IQueryable<Article> _articlesAsQueryable;

        [SetUp]
        public void Setup()
        {
            var articles = new List<Article>
            {
                new Article("rev", 19),
                new Article("style", 21),
                new Article("iuda", 18),
                new Article("Anna", 18)
            };
            
            _articlesAsQueryable = articles.AsQueryable();
        }

        [Test]
        public void InterpolationToConcatenation_OrderByInterpolationString_Correct()
        {
            var articles = _articlesAsQueryable
                .Select(x => $" Title:{x.Title} Cost {x.Cost}.")
                .InterpolationToConcatenation()
                .OrderBy(x => x);

            Assert.DoesNotThrow(() => articles.ToList());
        }
    }
}
