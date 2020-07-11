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
    public class CommandProcessorServer
    {
        private readonly ICommandProcessor _commandProcessor;
        private readonly ICommandTransportServer _commandTransportServer;

        public CommandProcessorServer(ICommandProcessor commandProcessor, ICommandTransportServer commandTransportServer)
        {
            _commandProcessor = commandProcessor;
            _commandTransportServer = commandTransportServer;
        }

        // TODO: should stop and start be called from transport
        public async Task StartAsync()
        {
            await _commandTransportServer.StartAsync().ConfigureAwait(false);

            // TODO: what to do with pending messages?
            // TODO: I guess these commands have to be able to be called many times
            // TODO: is the best way to poll with a delay?
            // TODO: stop or cancellation token?

            await _commandTransportServer.ListenAsync(async commandTransport =>
            {
                var commandObject = commandTransport.Command;

                var successResultType = commandObject.GetType().GetInterfaces().Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISuccessResult<,>));

                var method = _commandProcessor.GetType().GetMethod("ProcessAsync").MakeGenericMethod(successResultType.GenericTypeArguments[0], successResultType.GenericTypeArguments[1]);

                var resultTask = ((Task)(method.Invoke(_commandProcessor, new[] { commandObject })));

                var resultTransportType = typeof(ResultTransport<>).MakeGenericType(successResultType.GenericTypeArguments[1]);
                var resultTransport = resultTransportType.GetConstructor(Array.Empty<Type>()).Invoke(Array.Empty<object>());
                var idProperty = resultTransportType.GetProperty("Id");
                var successProperty = resultTransportType.GetProperty("Success");

                idProperty.SetValue(resultTransport, commandTransport.Id);

                try
                {
                    await resultTask.ConfigureAwait(false);
                    var taskResultProperty = resultTask.GetType().GetProperty("Result");

                    successProperty.SetValue(resultTransport, true);
                    var resultProperty = resultTransportType.GetProperty("Result");
                    resultProperty.SetValue(resultTransport, taskResultProperty.GetValue(resultTask));
                }
                catch (Exception exception)
                {
                    successProperty.SetValue(resultTransport, false);
                    var exceptionProperty = resultTransportType.GetProperty("Exception");
                    exceptionProperty.SetValue(resultTransport, exception);
                }

                await _commandTransportServer.SendAsync((IResultTransport)resultTransport).ConfigureAwait(false);

            }).ConfigureAwait(false);
        }

        public async Task StopAsync()
        {
            await _commandTransportServer.StopAsync().ConfigureAwait(false);
        }
    }
}
