using System.Threading.Tasks;

namespace CloudCommandsReader
{
    public interface ICommandReader
    {
        Task<CommandResult> ReadCommandAsync();
    }
}