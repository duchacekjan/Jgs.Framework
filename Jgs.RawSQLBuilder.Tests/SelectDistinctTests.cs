using FluentAssertions;
using Xunit;

namespace Jgs.RawSQLBuilder.Tests
{
    public class SelectDistinctTests
    {
        [Fact]
        public void CorrectSimpleSelectDistinct()
        {
            var actual = Query.Select().Distinct("a", "b", "c, d").From("t").SQL;
            actual.Should().Be("SELECT DISTINCT a, b, c, d FROM t");
        }

        [Fact]
        public void CorrectSelectDistinctWithAliases()
        {
            var actual = Query.Select()
                .Distinct("a as x", "b as y").Field("c").As("z").Fields("d as q")
                .From("t").SQL;
            actual.Should().Be("SELECT DISTINCT a as x, b as y, c as z, d as q FROM t");
        }

        [Fact]
        public void CorrectSelectCountDistinct()
        {
            var actual = Query.Select().CountDistinct("a").Field("b").As("x").From("t").SQL;
            actual.Should().Be("SELECT COUNT(DISTINCT a), b as x FROM t");
        }
    }
}
