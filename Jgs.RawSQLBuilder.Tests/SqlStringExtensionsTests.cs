using FluentAssertions;
using Jgs.RawSQLBuilder.Strings;
using System;
using Xunit;

namespace Jgs.RawSQLBuilder.Tests
{
    public class SqlStringExtensionsTests
    {

        [Fact]
        public void CorrectStringToSql()
        {
            var value = "a";
            const string expected = "'a'";
            var actual = value.ToSql();
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("%")]
        [InlineData("*")]
        public void CorrectStringContainsToSql(string wildCard)
        {
            var field = "abcd";
            const string expectedFormat = "abcd LIKE '{0}p{0}'";
            var expected = string.Format(expectedFormat, wildCard);
            var actual = field.SqlContains("p", wildCard);
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("%")]
        [InlineData("*")]
        public void CorrectStringStartsWithToSql(string wildCard)
        {
            var field = "abcd";
            const string expectedFormat = "abcd LIKE '{0}p'";
            var expected = string.Format(expectedFormat, wildCard);
            var actual = field.SqlStartsWith("p", wildCard);
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("%")]
        [InlineData("*")]
        public void CorrectStringEndsWithToSql(string wildCard)
        {
            var field = "abcd";
            const string expectedFormat = "abcd LIKE 'p{0}'";
            var expected = string.Format(expectedFormat, wildCard);
            var actual = field.SqlEndsWith("p", wildCard);
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("a", null, null)]
        [InlineData("a", null, "")]
        [InlineData("a", "", null)]
        [InlineData("a", "", "")]
        [InlineData("", null, null)]
        [InlineData("", null, "")]
        [InlineData("", "", null)]
        [InlineData("", "", "")]
        [InlineData(" ", null, null)]
        [InlineData(" ", null, "")]
        [InlineData(" ", "", null)]
        [InlineData(" ", "", "")]
        [InlineData(null, null, null)]
        [InlineData(null, null, "")]
        [InlineData(null, "", null)]
        [InlineData(null, "", "")]
        public void IncorrectWildCardInStringContainsToSql(string field, string value, string wildCard)
        {
            field.Invoking(i => i.SqlContains(value, wildCard))
                .Should()
                .Throw<ArgumentNullException>("Wild card is not defined.");
        }
    }
}
