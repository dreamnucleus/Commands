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
        private readonly IDatabase _database;

        public RedisCommandProcessorClient(ConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _database = _connectionMultiplexer.GetDatabase();
        }

        public async Task<TSuccessResult> ProcessAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            var resultTaskCompletionSource = new TaskCompletionSource<TSuccessResult>();

            var commandId = Guid.NewGuid().ToString();

            // push it out on a stream and await a response on pub sub
            var channel = await _connectionMultiplexer.GetSubscriber().SubscribeAsync(commandId).ConfigureAwait(false);

            channel.OnMessage(m =>
            {
                var resultObject = JsonConvert.DeserializeObject(m.Message, Constants.JsonSerializerSettings);

                if (resultObject is TSuccessResult result)
                {
                    resultTaskCompletionSource.SetResult(result);
                }
                else if (resultObject is Exception exception)
                {
                    resultTaskCompletionSource.SetException(exception);
                }
                else
                {
                    throw new NotImplementedException();
                }

                channel.Unsubscribe();
            });

            await _database.StreamAddAsync(Constants.Stream, commandId, JsonConvert.SerializeObject(command, Constants.JsonSerializerSettings)).ConfigureAwait(false);

            return await resultTaskCompletionSource.Task.ConfigureAwait(false);
        }

        public async Task<CommandProcessorSuccessResult<TSuccessResult>> ProcessResultAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            throw new NotImplementedException();
        }
    }
}
