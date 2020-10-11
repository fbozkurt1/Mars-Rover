using MarsRover.Surface;
using System;
using System.Collections.Generic;

namespace MarsRover.NasaRover
{
    public class Rover : IRover
    {
        #region Fields

        public Point Position { get; set; }

        public ISurface Surface { get; }

        public Direction Direction { get; private set; }

        #endregion

        #region Ctor

        public Rover(Point position, Direction direction, ISurface surface)
        {
            Position = position;
            Direction = direction;
            Surface = surface;
        }

        #endregion

        #region Methods

        public void Move(IEnumerable<Movement> movements)
        {
            foreach (var movement in movements)
            {
                switch (movement)
                {
                    case Movement.L:
                        TurnLeft();
                        break;
                    case Movement.R:
                        TurnRight();
                        break;
                    case Movement.M:
                        MoveForward();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Movement can be L, R or M. Current movement is {movement}");
                }
            }
        }

        private void TurnLeft()
        {
            Direction = Direction switch
            {
                Direction.N => Direction.W,
                Direction.W => Direction.S,
                Direction.S => Direction.E,
                Direction.E => Direction.N,
                _ => throw new ArgumentOutOfRangeException($"Direction can be N, S, W or E. Current direction is {Direction}")
            };
        }

        private void TurnRight()
        {
            Direction = Direction switch
            {
                Direction.N => Direction.E,
                Direction.E => Direction.S,
                Direction.S => Direction.W,
                Direction.W => Direction.N,
                _ => throw new ArgumentOutOfRangeException($"Direction can be N, S, W or E. Current direction is {Direction}")
            };
        }

        private void MoveForward()
        {
            switch (Direction)
            {
                case Direction.N:
                    if (Position.Y + 1 <= Surface.Size.Height)
                        Position = new Point(Position.X, Position.Y + 1);
                    break;
                case Direction.E:
                    if (Position.X + 1 <= Surface.Size.Width)
                        Position = new Point(Position.X + 1, Position.Y);
                    break;
                case Direction.S:
                    if (Position.Y - 1 >= 0)
                        Position = new Point(Position.X, Position.Y - 1);
                    break;
                case Direction.W:
                    if (Position.X - 1 >= 0)
                        Position = new Point(Position.X - 1, Position.Y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Direction can be N, S, W or E. Current direction is {Direction}");
            }
        }

        public override string ToString()
        {
            return $"{Position.X} {Position.Y} {Direction}";
        }

        #endregion
    }
}
