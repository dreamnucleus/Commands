using System.Collections.Concurrent;
using System.Linq;

namespace DreamNucleus.Commands.Extensions.Redis.Tests.Common
{
    public class InMemoryTransport
    {
        private readonly ConcurrentDictionary<string, ICommandContainer> _commandTransports;
        private readonly ConcurrentDictionary<string, IResultContainer> _resultTransports;

        public InMemoryTransport()
        {
            _commandTransports = new ConcurrentDictionary<string, ICommandContainer>();
            _resultTransports = new ConcurrentDictionary<string, IResultContainer>();
        }

        public bool TrySendCommand(ICommandContainer commandTransport)
        {
            return _commandTransports.TryAdd(commandTransport.Id, commandTransport);
        }

        public bool TryReadCommand(out ICommandContainer commandTransport)
        {
            if (_commandTransports.Any())
            {
                // TODO: fix thread safety
                return _commandTransports.TryRemove(_commandTransports.Keys.First(), out commandTransport);
            }
            else
            {
                commandTransport = default;
                return false;
            }
        }

        public bool TrySendResult(IResultContainer resultTransport)
        {
            return _resultTransports.TryAdd(resultTransport.Id, resultTransport);
        }

        public bool TryReadResult(string commandId, out IResultContainer resultTransport)
        {
            return _resultTransports.TryRemove(commandId, out resultTransport);
        }
    }
}
