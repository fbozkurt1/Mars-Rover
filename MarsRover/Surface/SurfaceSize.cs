namespace MarsRover.Surface
{
    public struct SurfaceSize
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public SurfaceSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
