// TODO: REMOVE
namespace DreamNucleus.Commands.Extensions.Unique
{
    // TODO: this could jst be a semaphore which doesn't expire
    public interface IUniqueCommand
    {
        string UniqueCommandId { get; }
    }
}
