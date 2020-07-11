using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DreamNucleus.Commands.Extensions.Redis
{
    public class CommandProcessorClient : ICommandProcessor
    {
        private readonly ICommandTransportClient _commandTransportClient;

        public CommandProcessorClient(ICommandTransportClient commandTransportClient)
        {
            _commandTransportClient = commandTransportClient;
        }

        public async Task<TSuccessResult> ProcessAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            var resultTaskCompletionSource = new TaskCompletionSource<TSuccessResult>();

            // TODO: timeout
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            cancellationTokenSource.Token.Register(() => resultTaskCompletionSource.TrySetException(new Exception("30b1ddaa-21d9-452d-8c32-faf070dc7c8c")));

            var commandTransport = new CommandTransport
            {
                Id = Guid.NewGuid().ToString(), // TODO: could i do a string ID which i pass in and then stop duplicates. For example the ID of the object to be created in the db
                Command = command
            };

            await _commandTransportClient.ListenAsync<TSuccessResult>(commandTransport.Id, resultTransport =>
            {
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

                return Task.CompletedTask;

            }).ConfigureAwait(false);

            await _commandTransportClient.SendAsync(commandTransport).ConfigureAwait(false);

            return await resultTaskCompletionSource.Task.ConfigureAwait(false);
        }

        public async Task<CommandProcessorSuccessResult<TSuccessResult>> ProcessResultAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            throw new NotImplementedException();
        }
    }
}
