using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DreamNucleus.Commands.Extensions.Redis
{
    public class RedisCommandTransportServer : ICommandTransportServer
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly string _streamName;
        private readonly string _consumerGroupName;
        private readonly string _consumerName;

        private readonly IDatabase _database;

        private readonly ConcurrentDictionary<string, (string ReturnChannelName, string MessageId)> _commandIdToMessageProperties;

        public RedisCommandTransportServer(IConnectionMultiplexer connectionMultiplexer, string streamName, string consumerGroupName, string consumerName)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _streamName = streamName;
            _consumerGroupName = consumerGroupName;
            _consumerName = consumerName;

            _database = _connectionMultiplexer.GetDatabase();

            _commandIdToMessageProperties = new ConcurrentDictionary<string, (string ReturnChannelName, string MessageId)>();
        }

        public async Task StartAsync()
        {
            // TODO: ensure it is a stream
            var streamExists = await _database.KeyExistsAsync(_streamName).ConfigureAwait(false);

            if (streamExists)
            {
                var streamGroupInfo = await _database.StreamGroupInfoAsync(_streamName).ConfigureAwait(false);

                if (streamGroupInfo.All(g => g.Name != _consumerGroupName))
                {
                    await _database.StreamCreateConsumerGroupAsync(_streamName, _consumerGroupName).ConfigureAwait(false);
                }
            }
            else
            {
                await _database.StreamCreateConsumerGroupAsync(_streamName, _consumerGroupName).ConfigureAwait(false);
            }
        }

        public async Task ListenAsync(Func<ICommandTransport, Task> listenFunc)
        {
            while (true)
            {
                var streamMessages = await _database.StreamReadGroupAsync(_streamName, _consumerGroupName, _consumerName, ">", count: 1).ConfigureAwait(false);

                if (streamMessages.Any())
                {
                    var streamMessage = streamMessages.Single();
                    var message = streamMessage.Values.Single();

                    var commandTransport = JsonConvert.DeserializeObject<ICommandTransport>(message.Value, Constants.JsonSerializerSettings);

                    if (!_commandIdToMessageProperties.TryAdd(commandTransport.Id, (message.Name.ToString(), streamMessage.Id)))
                    {
                        throw new NotImplementedException();
                    }

                    await listenFunc(commandTransport).ConfigureAwait(false);
                }

                // TODO: how best to loop
                await Task.Delay(100).ConfigureAwait(false);
            }
        }

        public async Task SendAsync(IResultTransport resultTransport)
        {
            // TODO: should be done in a transaction
            if (_commandIdToMessageProperties.TryRemove(resultTransport.Id, out var messageProperties))
            {
                await _database.StreamAcknowledgeAsync(_streamName, _consumerGroupName, messageProperties.MessageId).ConfigureAwait(false);

                await _database.PublishAsync(messageProperties.ReturnChannelName, JsonConvert.SerializeObject(resultTransport, Constants.JsonSerializerSettings)).ConfigureAwait(false);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task StopAsync()
        {
            throw new NotImplementedException();
        }
    }
}
