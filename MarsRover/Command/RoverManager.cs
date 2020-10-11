using MarsRover.NasaRover;
using MarsRover.Surface;
using System;
using System.Collections.Generic;

namespace MarsRover.Command
{
    public class RoverManager : IRoverManager
    {
        #region Fields
        public List<Rover> Rovers { get; } = new List<Rover>();
        public Rover ActiveRover { get; private set; }
        public ISurface Surface { get; }
        #endregion

        #region Ctor
        public RoverManager(ISurface surface)
        {
            Surface = surface;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Sets Rover position on the Plateau
        /// </summary>
        /// <param name="point"></param>
        /// <param name="direction"></param>
        public void DeployRover(Point point, Direction direction)
        {
            // Check rover position
            CheckIfPositionToDeployIsValid(point);

            // Create new Rover, Add it to list and set it as active
            var rover = new Rover(point, direction, Surface);
            Rovers.Add(rover);
            ActiveRover = rover;
        }

        /// <summary>
        /// Checks if Rover deploy position is in Plateau size.
        /// </summary>
        /// <param name="point"></param>
        private void CheckIfPositionToDeployIsValid(Point point)
        {
            bool isValid = (point.X >= 0 && point.X < Surface.Size.Width) && (point.Y >= 0 && point.Y < Surface.Size.Height);
            if (!isValid)
                throw new Exception($"Rover outside of bounds. X: {point.X}, Y:{point.Y}. Surface Width:{Surface.Size.Width}, Height:{Surface.Size.Height}");
        }

        #endregion
    }
}
