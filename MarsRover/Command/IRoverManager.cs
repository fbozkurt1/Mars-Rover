using MarsRover.NasaRover;
using MarsRover.Surface;
using System.Collections.Generic;

namespace MarsRover.Command
{
    public interface IRoverManager
    {
        List<Rover> Rovers { get; }

        Rover ActiveRover { get; }

        ISurface Surface { get; }

        void DeployRover(Point point, Direction direction);
    }
}
