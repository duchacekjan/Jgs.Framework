using FluentAssertions;
using Xunit;

namespace Jgs.RawSQLBuilder.Tests
{
    public class SelectTests
    {
        [Fact]
        public void CorrectSelectAll_ByAll()
        {
            var actual = Query.Select().All().From("table").SQL;
            actual.Should().Be("SELECT * FROM table");
        }

        [Fact]
        public void CorrectSelectAll_ByField()
        {
            var actual = Query.Select().Field("*").From("table").SQL;
            actual.Should().Be("SELECT * FROM table");
        }

        [Fact]
        public void CorrectSelectAll_ByFields()
        {
            var actual = Query.Select().Fields("*").From("table").SQL;
            actual.Should().Be("SELECT * FROM table");
        }

        [Fact]
        public void CorrectSelectAll()
        {
            var actual = Query.Select("*").From("table").SQL;
            actual.Should().Be("SELECT * FROM table");
        }

        [Fact]
        public void CorrectSelectCount()
        {
            var actual = Query.Select()
                .Count("*").From("table").SQL;
            actual.Should().Be("SELECT COUNT(*) FROM table");
        }

        [Fact]
        public void CorrectSelectCountAs()
        {
            var actual = Query.Select()
                .Count("*").As("cnt")
                .From("table").SQL;
            actual.Should().Be("SELECT COUNT(*) as cnt FROM table");
        }

        [Fact]
        public void CorrectSelectFieldAndCount()
        {
            var actual = Query.Select()
                .Field("f").As("x")
                .Count("*").From("table").SQL;
            actual.Should().Be("SELECT f as x, COUNT(*) FROM table");
        }

        [Fact]
        public void CorrectSelectFieldAndCountAs()
        {
            var actual = Query.Select()
                .Field("f").As("x")
                .Count("*").As("cnt")
                .From("table").SQL;
            actual.Should().Be("SELECT f as x, COUNT(*) as cnt FROM table");
        }
    }
}
