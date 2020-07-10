﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DreamNucleus.Commands.Extensions.Redis
{
    public class RedisCommandProcessorServer
    {
        private readonly ICommandProcessor _commandProcessor;
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private readonly string _consumerGroupName;
        private readonly string _consumerName;
        private readonly IDatabase _database;

        public RedisCommandProcessorServer(ICommandProcessor commandProcessor, ConnectionMultiplexer connectionMultiplexer, string consumerGroupName, string consumerName)
        {
            _commandProcessor = commandProcessor;
            _connectionMultiplexer = connectionMultiplexer;
            _consumerGroupName = consumerGroupName;
            _consumerName = consumerName;
            _database = _connectionMultiplexer.GetDatabase();
        }

        public async Task Start()
        {
            var streamGroupInfo = await _database.StreamGroupInfoAsync(Constants.Stream).ConfigureAwait(false);

            if (streamGroupInfo.All(g => g.Name != _consumerGroupName))
            {
                await _database.StreamCreateConsumerGroupAsync(Constants.Stream, _consumerGroupName).ConfigureAwait(false);
            }

            while (true)
            {
                var streamMessages = await _database.StreamReadGroupAsync(Constants.Stream, _consumerGroupName, _consumerName, ">", count: 1).ConfigureAwait(false);

                if (streamMessages.Any())
                {
                    var streamMessage = streamMessages.Single();
                    var message = streamMessage.Values.Single();

                    var commandTransport = JsonConvert.DeserializeObject<CommandTransport>(message.Value, Constants.JsonSerializerSettings);
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

                    await _database.StreamAcknowledgeAsync(Constants.Stream, _consumerGroupName, streamMessage.Id).ConfigureAwait(false);

                    await _database.PublishAsync(message.Name.ToString(), JsonConvert.SerializeObject(resultTransport, Constants.JsonSerializerSettings)).ConfigureAwait(false);
                }

                await Task.Delay(1_000).ConfigureAwait(false);
            }
        }
    }
}
