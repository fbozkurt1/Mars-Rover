using MarsRover.NasaRover;
using MarsRover.Surface;
using System;
using System.Linq;

namespace MarsRover.Command
{
    public class RoverDeployCommandExecuter : CommandExecuter
    {
        #region Fields

        private readonly IRoverManager _roverManager;
        private static Point point;
        private static Direction direction;

        #endregion

        #region Ctor

        public RoverDeployCommandExecuter(IServiceProvider serviceProvider)
        {
            _roverManager = (IRoverManager)serviceProvider.GetService(typeof(IRoverManager));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes Command to Deploy Rover
        /// </summary>
        /// <param name="command"></param>
        public override void Execute(string command)
        {
            ParseCommand(command);
            _roverManager.DeployRover(point, direction);
        }

        /// <summary>
        /// Checks if command is appropriate to Deploy Rover
        /// And also used to find out which command executer to run
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public override bool IsCommandBelongsToThisExecuter(string command)
        {
            var args = command.Trim().Split(' ').ToList();
            if (args.Count != 3)
                return false;

            // First two inputs should be integer and greater than or equal 0, Third input should be letter (N,S,W,E).
            var isRoverXCoordinateValid = int.TryParse(args[0], out int xPosition) && xPosition >= 0;
            var isRoverYCoordinateValid = int.TryParse(args[1], out int yPosition) && yPosition >= 0;
            var isRoverDirectionValid = Enum.TryParse(typeof(Direction), args[2], true, out _);

            return isRoverXCoordinateValid && isRoverYCoordinateValid && isRoverDirectionValid;
        }

        /// <summary>
        /// Divides command to three pieces that are X coordinate, Y coordinate and direction to set Initial Rover position.
        /// </summary>
        /// <param name="command"></param>
        private static void ParseCommand(string command)
        {
            var splittedCommand = command.Split(' ');
            point = new Point(int.Parse(splittedCommand[0]), int.Parse(splittedCommand[1]));
            direction = Enum.Parse<Direction>(splittedCommand[2], true);
        }

        #endregion
    }
}
