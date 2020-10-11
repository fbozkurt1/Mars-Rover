namespace MarsRover.Surface
{
    public interface ISurface
    {
        SurfaceSize Size { get; }
        void SetSize(int width, int height);
    }
}
