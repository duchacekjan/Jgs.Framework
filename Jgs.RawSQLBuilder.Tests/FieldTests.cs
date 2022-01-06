using FluentAssertions;
using Jgs.RawSQLBuilder.Core;
using System;
using Xunit;

namespace Jgs.RawSQLBuilder.Tests
{
#pragma warning disable CA1806 // Do not ignore method results
    public class FieldTests
    {
        [Fact]
        public void ShouldCastFromString()
        {
            var sut = new Field("name");
            sut.Name.Should().Be("name");
            sut.Alias.Should().BeEmpty();
            sut.SQL.Should().Be("name");
        }

        [Fact]
        public void ShouldCastFromStringWithAlias()
        {
            var sut = new Field("name as caption");

            sut.Name.Should().Be("name");
            sut.Alias.Should().Be("caption");
            sut.SQL.Should().Be("name as caption");
        }

        [Fact]
        public void ShouldImplicitCastFromString()
        {
            Field sut = "name";
            sut.Name.Should().Be("name");
            sut.Alias.Should().BeEmpty();
            sut.SQL.Should().Be("name");
        }

        [Fact]
        public void ShouldImplicitCastFromStringWithAlias()
        {
            Field sut = "name as caption";

            sut.Name.Should().Be("name");
            sut.Alias.Should().Be("caption");
            sut.SQL.Should().Be("name as caption");
        }

        [Fact]
        public void EmptyFieldName()
        {
            Action sut = () => new Field(string.Empty);

            sut.Should()
                .Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(" as ")]
        [InlineData("  as ")]
        [InlineData("name long as caption")]
        [InlineData("name caption")]
        [InlineData("%^&({}+-/ ]['')")]

        public void WrongFormatFieldName(string fieldName)
        {
            Action sut = () => new Field(fieldName);

            sut.Should()
                .Throw<ArgumentException>();
        }
    }
#pragma warning restore CA1806 // Do not ignore method results
}
