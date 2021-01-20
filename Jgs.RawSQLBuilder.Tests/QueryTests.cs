using FluentAssertions;
using Xunit;

namespace Jgs.RawSQLBuilder.Tests
{
    public class QueryTests
    {
        [Fact]
        public void ShouldBeCorrectSimpleSelectClause()
        {
            var actual = Query
                .Select("a")
                .From("t")
                .SQL;

            actual.Should().Be("SELECT a FROM t");
        }

        [Fact]
        public void ShouldBeCorrectSelectClauseWithWhere()
        {
            var actual = Query
                .Select("a")
                .From("t")
                .Where("1=0")
                .SQL;

            actual.Should().Be("SELECT a FROM t WHERE 1=0");
        }
    }
}
