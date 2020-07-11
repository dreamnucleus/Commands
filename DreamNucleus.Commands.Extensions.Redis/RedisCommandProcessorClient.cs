using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DreamNucleus.Commands.Extensions.Redis
{
    public class RedisCommandProcessorClient : ICommandProcessor
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private readonly string _streamName;
        private readonly string _channelName;
        private readonly IDatabase _database;

        public RedisCommandProcessorClient(ConnectionMultiplexer connectionMultiplexer, string streamName, string channelName)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _streamName = streamName;
            _channelName = channelName;
            _database = _connectionMultiplexer.GetDatabase();
        }

        public async Task<TSuccessResult> ProcessAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            var resultTaskCompletionSource = new TaskCompletionSource<TSuccessResult>();

            var commandTransport = new CommandTransport
            {
                Id = Guid.NewGuid(), // TODO: could i do a string ID which i pass in and then stop duplicates. For example the ID of the object to be created in the db
                Command = command
            };

            // push it out on a stream and await a response on pub sub
            // TODO: another class should be always watching this and pushing it out so we don't need to sub and un sub
            var channel = await _connectionMultiplexer.GetSubscriber().SubscribeAsync(_channelName).ConfigureAwait(false);

            channel.OnMessage(async m =>
            {
                var resultTransport = JsonConvert.DeserializeObject<ResultTransport<TSuccessResult>>(m.Message, Constants.JsonSerializerSettings);

                if (resultTransport.Id == commandTransport.Id)
                {
                    await channel.UnsubscribeAsync().ConfigureAwait(false);

                    if (resultTransport.Success)
                    {
                        resultTaskCompletionSource.SetResult(resultTransport.Result);
                    }
                    else if (!resultTransport.Success)
                    {
                        resultTaskCompletionSource.SetException(resultTransport.Exception);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                }
            });

            // TODO: stream max length?
            await _database.StreamAddAsync(_streamName, _channelName, JsonConvert.SerializeObject(commandTransport, Constants.JsonSerializerSettings)).ConfigureAwait(false);

            return await resultTaskCompletionSource.Task.ConfigureAwait(false);
        }

        public async Task<CommandProcessorSuccessResult<TSuccessResult>> ProcessResultAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            throw new NotImplementedException();
        }
    }
}
