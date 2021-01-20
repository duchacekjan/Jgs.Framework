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

        [Fact]
        public void ShouldBeCorrectSelectClauseWithComplexWhere()
        {
            var actual = Query
                .Select("a")
                .From("t")
                .Where("1=0").And("a=5").Or("b=0").And("")
                .SQL;

            actual.Should().Be("SELECT a FROM t WHERE 1=0 AND a=5 OR b=0 AND 1=1");
        }

        [Fact]
        public void ShouldBeCorrectSelectClauseWithWhereExists()
        {
            var actual = Query
                .Select("a")
                .From("t")
                .WhereExists("select * from data where data.a=b")
                .SQL;

            actual.Should().Be("SELECT a FROM t WHERE EXISTS (select * from data where data.a=b)");
        }

        [Fact]
        public void ShouldBeCorrectSelectClauseWithWhereNotExists()
        {
            var actual = Query
                .Select("a")
                .From("t")
                .WhereNotExists("select * from data where data.a=b")
                .SQL;

            actual.Should().Be("SELECT a FROM t WHERE NOT EXISTS (select * from data where data.a=b)");
        }

        [Fact]
        public void ShouldBeCorrectSelectClauseWithWhereComplexExists()
        {
            var actual = Query
                .Select("a")
                .From("t")
                .Where("1=0").AndNotExists("a").OrExists("b")
                .SQL;

            actual.Should().Be("SELECT a FROM t WHERE 1=0 AND (NOT EXISTS (a)) OR (EXISTS (b))");
        }

        [Fact]
        public void ShouldBeCorrectSelectClauseWithDistinct()
        {
            //var actual = Query
            //    .SelectDistinct("a")
            //    .From("t")
            //    .SQL;

            //actual.Should().Be("SELECT DISTINCT a FROM t");
        }
    }
}
