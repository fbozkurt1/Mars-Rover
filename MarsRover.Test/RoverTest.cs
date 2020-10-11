using FluentAssertions;
using MarsRover.Command;
using MarsRover.NasaRover;
using MarsRover.Surface;
using System;
using System.Linq;
using Xunit;

namespace MarsRover.Test
{
    public class RoverTest
    {
        #region Rover Deploying Tests

        [Theory]
        [InlineData("1 2 N")]
        [InlineData("4 1 S")]
        [InlineData("2 2 W")]
        [InlineData("1 3 E")]
        public void Rover_Is_Deployed_In_Acceptable_Position_On_Plateau(string command)
        {
            // Arrange
            ISurface plateau = new Plateau();
            plateau.SetSize(5, 5);

            IRoverManager roverManager = new RoverManager(plateau);
            var commandSplitted = command.Split(' ');
            var expectedXCoordinate = int.Parse(commandSplitted[0]);
            var expectedYCoordinate = int.Parse(commandSplitted[1]);
            var expectedDirection = Enum.Parse<Direction>(commandSplitted[2]);

            // Act
            roverManager.DeployRover(new Point(expectedXCoordinate, expectedYCoordinate), expectedDirection);

            // Assert
            roverManager.ActiveRover.Position.X.Should().Be(expectedXCoordinate);
            roverManager.ActiveRover.Position.Y.Should().Be(expectedYCoordinate);
            roverManager.ActiveRover.Direction.Should().Be(expectedDirection);
        }

        [Theory]
        [InlineData("1 2 N")]
        [InlineData("4 1 S")]
        [InlineData("2 2 W")]
        [InlineData("1 3 E")]
        public void Rover_Is_Deployed_In_UnAcceptable_Position_On_Plateau(string command)
        {
            // Arrange
            ISurface plateau = new Plateau();
            plateau.SetSize(1, 1);

            IRoverManager roverManager = new RoverManager(plateau);
            var commandSplitted = command.Split(' ');
            var expectedXCoordinate = int.Parse(commandSplitted[0]);
            var expectedYCoordinate = int.Parse(commandSplitted[1]);
            var expectedDirection = Enum.Parse<Direction>(commandSplitted[2]);

            // Act
            var action = new Action(() => roverManager.DeployRover(new Point(expectedXCoordinate, expectedYCoordinate), expectedDirection));

            // Assert
            action.Should().Throw<Exception>().WithMessage($"Rover outside of bounds. X: {expectedXCoordinate}, Y:{expectedYCoordinate}. Surface Width:{plateau.Size.Width}, Height:{plateau.Size.Height}");
        }

        #endregion

        #region Rover Movement Tests

        [Theory]
        [InlineData("MMRMMM")]
        public void Rover_Moves_Two_Units_North_Three_Units_East_On_Plateau(string command)
        {
            // Arrange
            ISurface plateau = new Plateau();
            plateau.SetSize(5, 5);

            IRoverManager roverManager = new RoverManager(plateau);
            roverManager.DeployRover(new Point(0, 0), Direction.N);
            var movements = command.ToCharArray().Select(x => Enum.Parse<Movement>(x.ToString())).ToList();

            //Act
            roverManager.ActiveRover.Move(movements);

            //Assert
            roverManager.ActiveRover.Should().NotBeNull();
            roverManager.ActiveRover.Position.Should().Be(new Point(3, 2));
            roverManager.ActiveRover.Direction.Should().Be(Direction.E);
        }

        [Theory]
        [InlineData("MMMMM")]
        public void Rover_Moves_To_North_More_Than_Plateau_Height_Returns_X_Position_Maximum_Plateau_Height(string command)
        {
            // Arrange
            ISurface plateau = new Plateau();
            plateau.SetSize(5, 5);
            IRoverManager roverManager = new RoverManager(plateau);
            roverManager.DeployRover(new Point(0, 0), Direction.N);
            var movements = command.ToCharArray().Select(x => Enum.Parse<Movement>(x.ToString())).ToList();

            //Act
            roverManager.ActiveRover.Move(movements);

            //Assert
            roverManager.ActiveRover.Should().NotBeNull();
            roverManager.ActiveRover.Position.Should().Be(new Point(0, 5));
            roverManager.ActiveRover.Direction.Should().Be(Direction.N);
        }

        #endregion
    }
}
