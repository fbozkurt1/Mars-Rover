namespace MarsRover.Command
{
    public interface ICommandExecuter
    {
        void Execute(string command);
        bool IsCommandBelongsToThisExecuter(string command);
    }
}