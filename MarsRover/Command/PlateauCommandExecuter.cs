using MarsRover.Surface;
using System;
using System.Linq;

namespace MarsRover.Command
{
    public class PlateauCommandExecuter : CommandExecuter
    {
        #region Fields

        private readonly ISurface _surface;
        private static int width, height;

        #endregion

        #region Ctor
        public PlateauCommandExecuter(IServiceProvider serviceProvider)
        {
            _surface = (ISurface)serviceProvider.GetService(typeof(ISurface));
        }
        #endregion

        #region Methods

        /// <summary>
        /// Execute command to set Plateau Size
        /// </summary>
        /// <param name="command"></param>
        public override void Execute(string command)
        {
            ParseCommand(command);
            _surface.SetSize(width, height);
        }

        /// <summary>
        /// Checks if command is appropriate to Set Plateau Size
        /// And also used to find out which command executer to run
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public override bool IsCommandBelongsToThisExecuter(string command)
        {
            var args = command.Trim().Split(' ').ToList();
            if (args.Count != 2)
                return false;
            // they have to be integer and greater than 0
            var isPlateauWidthValid = int.TryParse(args[0], out int width) && width > 0;
            var isPlateauHeightValid = int.TryParse(args[1], out int height) && height > 0;

            return isPlateauWidthValid && isPlateauHeightValid;
        }

        /// <summary>
        /// Divide command to two pieces that are Width and Height to set Plateau Size.
        /// </summary>
        /// <param name="command"></param>
        private static void ParseCommand(string command)
        {
            var splitCommand = command.Split(' ');
            // We set them plus one because of X and Y co-ordinates are zero based.
            width = int.Parse(splitCommand[0]) + 1;
            height = int.Parse(splitCommand[1]) + 1;
        }

        #endregion
    }
}
