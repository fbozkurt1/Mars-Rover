using System;

namespace MarsRover.Surface
{
    public class Plateau : ISurface
    {
        #region Fields
        public SurfaceSize Size { get; private set; }
        #endregion

        #region Methods
        public void SetSize(int width, int height)
        {
            if (width < 1 || height < 1)
                throw new ArgumentOutOfRangeException($"Plateau width and height must be greater than 0(zero). Current Width: {width}, Height: {height}");

            Size = new SurfaceSize(width, height);
        }
        #endregion
    }
}
