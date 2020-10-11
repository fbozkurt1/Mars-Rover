using MarsRover.Surface;
using System.Collections.Generic;

namespace MarsRover.NasaRover
{
    public interface IRover
    {
        Point Position { get; set; }

        ISurface Surface { get; }

        Direction Direction { get; }

        void Move(IEnumerable<Movement> movements);
    }
}
