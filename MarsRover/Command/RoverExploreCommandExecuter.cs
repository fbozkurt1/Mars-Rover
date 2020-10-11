using MarsRover.NasaRover;
using System;
using System.Linq;

namespace MarsRover.Command
{
    public class RoverExploreCommandExecuter : CommandExecuter
    {
        #region Fields

        private readonly IRoverManager _roverManager;
        private Rover activeRover;

        #endregion

        #region Ctor
        public RoverExploreCommandExecuter(IServiceProvider serviceProvider)
        {
            _roverManager = (IRoverManager)serviceProvider.GetService(typeof(IRoverManager));
        }
        #endregion

        #region Methods

        /// <summary>
        /// Executes command for Rover to explore Plateau
        /// </summary>
        /// <param name="command"></param>
        public override void Execute(string command)
        {
            // Check if any rover deployed. Before exploring, rover's position must be deployed.
            if (CheckIfActiveRoverDeployed())
                return;

            // parse each movement command to Movement Enum
            var movements = command.Select(x => Enum.Parse<Movement>(x.ToString(), true));
            activeRover.Move(movements);

            // When exploring is finished print Rover's current position 
            Console.WriteLine(activeRover.ToString());
        }

        /// <summary>
        /// Checks if command is appropriate Rover to explore Plateau
        /// and also used to find out which command executer to run
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public override bool IsCommandBelongsToThisExecuter(string command)
        {
            var isAllLetter = command.All(char.IsLetter);
            if (!isAllLetter)
                return false;

            foreach (var movement in command)
            {
                if (!Enum.TryParse(typeof(Movement), movement.ToString(), true, out var result))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if any rover deployed
        /// </summary>
        /// <returns></returns>
        private bool CheckIfActiveRoverDeployed()
        {
            activeRover = _roverManager.ActiveRover;
            return activeRover == null;
        }

        #endregion
    }
}
