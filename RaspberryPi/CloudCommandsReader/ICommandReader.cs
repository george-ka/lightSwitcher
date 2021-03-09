namespace CloudCommandsReader
{
    public interface ICommandReader
    {
        Task<CommandResult> ReadCommandAsync();
    }
}