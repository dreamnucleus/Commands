﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DreamNucleus.Commands.Extensions.Redis
{
    public class RedisCommandTransportClient : ICommandTransportClient
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly string _streamName;
        private readonly string _channelName;

        private readonly IDatabase _database;
        
        public RedisCommandTransportClient(IConnectionMultiplexer connectionMultiplexer, string streamName, string channelName)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _streamName = streamName;
            _channelName = channelName;

            _database = _connectionMultiplexer.GetDatabase();
        }

        public async Task ListenAsync<TSuccessResult>(string commandId, Func<ResultTransport<TSuccessResult>, Task> listenFunc)
        {
            // push it out on a stream and await a response on pub sub
            // TODO: another class should be always watching this and pushing it out so we don't need to sub and un sub
            var channel = await _connectionMultiplexer.GetSubscriber().SubscribeAsync(_channelName).ConfigureAwait(false);

            channel.OnMessage(async m =>
            {
                var resultTransport = JsonConvert.DeserializeObject<IResultTransport>(m.Message, Constants.JsonSerializerSettings);

                if (resultTransport.Id == commandId)
                {
                    await channel.UnsubscribeAsync().ConfigureAwait(false);

                    await listenFunc((ResultTransport<TSuccessResult>)resultTransport).ConfigureAwait(false);
                }
            });
        }

        public async Task SendAsync(CommandTransport commandTransport)
        {
            // TODO: stream max length?
            await _database.StreamAddAsync(_streamName, _channelName, JsonConvert.SerializeObject(commandTransport, Constants.JsonSerializerSettings)).ConfigureAwait(false);
        }
    }
}
