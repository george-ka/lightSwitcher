namespace CloudCommandsReader
{
    public class CommandResult
    {
        public CommandResult(CommandReadingStatus status, byte command)
        {
            Status = status;
            Command = command;
        }

        public CommandReadingStatus Status {get; private set;}
        public byte Command { get; private set; }

        public static CommandResult CreateNoCommandResult()
        {
            return new CommandResult(CommandReadingStatus.NoCommand, 0);
        }

        public static CommandResult CreateTryAgainResult()
        {
            return new CommandResult(CommandReadingStatus.Retry, 0);
        }       
    }
}