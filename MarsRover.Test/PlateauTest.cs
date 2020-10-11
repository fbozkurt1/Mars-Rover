using FluentAssertions;
using MarsRover.Surface;
using System;
using Xunit;

namespace MarsRover.Test
{
    public class PlateauTest
    {
        [Theory]
        [InlineData("6 6")]
        [InlineData("1 5")]
        [InlineData("2 4")]
        [InlineData("2 10")]
        [InlineData("5 1")]
        public void When_Size_Has_Been_Set_Returns_Same_Size(string plateauSize)
        {
            // Arrange
            ISurface surface = new Plateau();

            var splittedSizes = plateauSize.Split(' ');
            int expectedWidth = int.Parse(splittedSizes[0]) + 1;
            int expectedHeight = int.Parse(splittedSizes[1]) + 1;

            // Act
            surface.SetSize(expectedWidth, expectedHeight);

            // Assert
            Assert.Equal(expectedWidth, surface.Size.Width);
            Assert.Equal(expectedHeight, surface.Size.Height);
        }

        [Theory]
        [InlineData("0 -1")]
        [InlineData("-1 0")]
        [InlineData("-2 -2")]
        public void When_Size_Has_Been_Set_With_Negative_Values_Throws_Exception(string plateauSize)
        {
            // Arrange
            ISurface surface = new Plateau();

            var splittedSizes = plateauSize.Split(' ');
            int expectedWidth = int.Parse(splittedSizes[0]) + 1;
            int expectedHeight = int.Parse(splittedSizes[1]) + 1;

            // Act
            var action = new Action(() => surface.SetSize(expectedWidth, expectedHeight));

            // Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
