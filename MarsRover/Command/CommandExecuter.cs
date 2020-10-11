namespace MarsRover.Command
{
    public abstract class CommandExecuter : ICommandExecuter
    {
        public abstract void Execute(string command);
        public abstract bool IsCommandBelongsToThisExecuter(string command);
    }
}
