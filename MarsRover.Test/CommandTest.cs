using FluentAssertions;
using MarsRover.Command;
using MarsRover.NasaRover;
using MarsRover.Surface;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace MarsRover.Test
{
    public class CommandTest
    {
        #region Fields

        private readonly IServiceProvider _serviceProvider;
        private readonly ICommand _command;

        #endregion

        #region Ctor
        public CommandTest()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ISurface, Plateau>()
                .AddSingleton<IRoverManager, RoverManager>()
                .BuildServiceProvider();
            _serviceProvider = serviceProvider;
            _command = new Command.Command(serviceProvider);
        }
        #endregion

        #region Plateau Command Executer Tests

        [Theory]
        [InlineData("5 5")]
        [InlineData("1 5")]
        [InlineData("3 4")]
        [InlineData("5 1")]
        public void Create_Plateau_With_Valid_Command(string command)
        {
            // Arrange
            var plateau = _serviceProvider.GetService<ISurface>();

            var commandSplitted = command.Split(' ');
            var expectedWidth = int.Parse(commandSplitted[0]) + 1;
            var expectedHeight = int.Parse(commandSplitted[1]) + 1;

            // Act
            _command.SendCommand(command);

            // Assert
            plateau.Should().NotBeNull();
            plateau.Size.Should().NotBeNull();
            plateau.Size.Width.Should().Be(expectedWidth);
            plateau.Size.Height.Should().Be(expectedHeight);
        }

        [Theory]
        [InlineData("-1 5")]
        [InlineData("0 -2")]
        [InlineData("-1 -1")]
        [InlineData("a b")]
        [InlineData("N S")]
        [InlineData("1 S")]
        [InlineData("S 1")]
        public void Create_Plateau_With_Invalid_Command_Throws_Exception(string command)
        {
            // Arrange
            var plateau = _serviceProvider.GetService<ISurface>();

            // Act
            var action = new Action(() => _command.SendCommand(command));

            // Assert
            action.Should().Throw<Exception>().WithMessage($"This ({command}) input is not valid. Please check your input. (You can consider test inputs)");
        }

        #endregion

        #region Rover Deploy Command Executer Tests

        [Theory]
        [InlineData("2 3 N")]
        [InlineData("3 2 E")]
        [InlineData("4 4 S")]
        [InlineData("5 5 W")]
        public void Rover_Deploy_With_Valid_Command(string deployCommand)
        {
            // Arrange
            _command.SendCommand("5 5"); // Create Plateau
            var roverManager = _serviceProvider.GetService<IRoverManager>();
            var deployCommandSplitted = deployCommand.Split(' ');
            var expectedXCoordinate = int.Parse(deployCommandSplitted[0]);
            var expectedYCoordinate = int.Parse(deployCommandSplitted[1]);
            var expectedDirection = Enum.Parse<Direction>(deployCommandSplitted[2].ToString(), true);

            // Act
            _command.SendCommand(deployCommand); // Explore command

            // Assert
            roverManager.ActiveRover.Should().NotBeNull();
            roverManager.ActiveRover.Position.X.Should().Be(expectedXCoordinate);
            roverManager.ActiveRover.Position.Y.Should().Be(expectedYCoordinate);
            roverManager.ActiveRover.Direction.Should().Be(expectedDirection);
        }

        [Theory]
        [InlineData("1 1 n")]
        [InlineData("1 1 e")]
        [InlineData("1 1 s")]
        [InlineData("1 1 w")]
        public void Rover_Deploy_With_LowerCase_Direction_Letter_Command(string deployCommand)
        {
            // Arrange
            _command.SendCommand("5 5"); // Create Plateau
            var roverManager = _serviceProvider.GetService<IRoverManager>();
            var deployCommandSplitted = deployCommand.Split(' ');
            var expectedXCoordinate = int.Parse(deployCommandSplitted[0]);
            var expectedYCoordinate = int.Parse(deployCommandSplitted[1]);
            var expectedDirection = Enum.Parse<Direction>(deployCommandSplitted[2].ToString(), true);

            // Act
            _command.SendCommand(deployCommand); // Explore command

            // Assert
            roverManager.ActiveRover.Should().NotBeNull();
            roverManager.ActiveRover.Position.X.Should().Be(expectedXCoordinate);
            roverManager.ActiveRover.Position.Y.Should().Be(expectedYCoordinate);
            roverManager.ActiveRover.Direction.Should().Be(expectedDirection);
        }

        [Theory]
        [InlineData("1 1", "1 2 N")]
        [InlineData("1 1", "2 1 N")]
        public void Rover_Deploy_Out_Of_Bounds_Throws_Exception(string plateauCreateCommand, string roverDeployCommand)
        {
            // Arrange
            _command.SendCommand(plateauCreateCommand); // creates plateau
            var plateau = _serviceProvider.GetService<ISurface>();
            var deployCommandSplitted = roverDeployCommand.Split(' ');
            var expectedXCoordinate = int.Parse(deployCommandSplitted[0]);
            var expectedYCoordinate = int.Parse(deployCommandSplitted[1]);

            // Act
            var action = new Action(() => _command.SendCommand(roverDeployCommand));

            // Assert
            action.Should().Throw<Exception>().WithMessage($"Rover outside of bounds. X: {expectedXCoordinate}, Y:{expectedYCoordinate}. Surface Width:{plateau.Size.Width}, Height:{plateau.Size.Height}");
        }

        [Fact]
        public void When_There_Is_No_Active_Rover_Dont_Act()
        {
            // Arrange
            _command.SendCommand("4 4"); // Creates Plateau
            var roverManager = _serviceProvider.GetService<IRoverManager>();

            // Act
            _command.SendCommand("MLMLMLM"); // Send command Rover to move

            //Assert
            roverManager.ActiveRover.Should().BeNull();
        }

        [Theory]
        [InlineData("1 1 A")]
        [InlineData("-1 1 N")]
        public void Rover_Deploy_With_Invalid_Command_Throws_Exception(string deployCommand)
        {
            // Arrange
            _command.SendCommand("5 5"); // Create Plateau

            // Act
            var action = new Action(() => _command.SendCommand(deployCommand)); // Deploy Rover

            // Assert
            action.Should().Throw<Exception>().WithMessage($"This ({deployCommand}) input is not valid. Please check your input. (You can consider test inputs)");
        }

        #endregion

        #region Rover Explore Command Executer Tests

        [Theory]
        [InlineData("MRMRMRMR")]
        [InlineData("MLMLMLML")]
        public void Command_To_Rover_Explore_In_A_Circle_Returns_To_Initial_Position(string command)
        {
            // Arrange
            _command.SendCommand("6 6"); // Create Plateau
            _command.SendCommand("1 1 N"); // Deploy Rover
            var rover = _serviceProvider.GetService<IRoverManager>().ActiveRover;

            // Act
            _command.SendCommand(command);

            //Assert
            rover.Should().NotBeNull();
            rover.Position.Should().Be(new Point(1, 1));
            rover.Direction.Should().Be(Direction.N);
        }

        [Theory]
        [InlineData("mmrmml")]
        public void Rover_Explore_With_LowerCase_Letter_Command(string exploreCommand)
        {
            // Arrange
            _command.SendCommand("5 5"); // Create Plateau
            _command.SendCommand("1 1 N"); // Deploy Rover
            var roverManager = _serviceProvider.GetService<IRoverManager>();

            // Act
            _command.SendCommand(exploreCommand); // Explore command

            // Assert
            roverManager.ActiveRover.Should().NotBeNull();
            roverManager.ActiveRover.Position.X.Should().Be(3);
            roverManager.ActiveRover.Position.Y.Should().Be(3);
            roverManager.ActiveRover.Direction.Should().Be(Direction.N);
        }

        [Theory]
        [InlineData("MLRS")]
        [InlineData("MRML1")]
        public void Rover_Explore_With_Invalid_Command_Throws_Exception(string exploreCommand)
        {
            // Arrange
            _command.SendCommand("5 5"); // Create Plateau
            _command.SendCommand("1 1 N"); // Deploy Rover

            // Act
            var action = new Action(() => _command.SendCommand(exploreCommand)); // Explore command

            // Assert
            action.Should().Throw<Exception>().WithMessage($"This ({exploreCommand}) input is not valid. Please check your input. (You can consider test inputs)");
        }

        #endregion
    }
}
