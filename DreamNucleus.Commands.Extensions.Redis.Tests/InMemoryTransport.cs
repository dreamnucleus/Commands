using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Extensions.Redis.Tests
{
    public class InMemoryTransport
    {
        private readonly ConcurrentDictionary<string, ICommandTransport> _commandTransports;
        private readonly ConcurrentDictionary<string, IResultTransport> _resultTransports;

        public InMemoryTransport()
        {
            _commandTransports = new ConcurrentDictionary<string, ICommandTransport>();
            _resultTransports = new ConcurrentDictionary<string, IResultTransport>();
        }

        public bool TrySendCommand(ICommandTransport commandTransport)
        {
            return _commandTransports.TryAdd(commandTransport.Id, commandTransport);
        }

        public bool TryReadCommand(out ICommandTransport commandTransport)
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

        public bool TrySendResult(IResultTransport resultTransport)
        {
            return _resultTransports.TryAdd(resultTransport.Id, resultTransport);
        }

        public bool TryReadResult(string commandId, out IResultTransport resultTransport)
        {
            return _resultTransports.TryRemove(commandId, out resultTransport);
        }
    }
}
