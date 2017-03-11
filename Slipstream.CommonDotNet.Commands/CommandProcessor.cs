﻿using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands
{
    public class CommandProcessor : ICommandProcessor
    {
        public Guid Id { get; } = Guid.NewGuid();


        private readonly ILifetimeScopeDependencyService dependencyService;
        public CommandProcessor(ILifetimeScopeService lifetimeScopeService)
        {
            Console.WriteLine("CommandProcessor created " + Id);
            this.dependencyService = lifetimeScopeService.BeginLifetimeScope(this);
        }

        public async Task<TSuccessResult> ProcessAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            //var validationContext = new ValidationContext(command);
            //var validationResults = new List<ValidationResult>();

            //var isValid = Validator.TryValidateobject(command, validationContext, validationResults, true);

            //if (!isValid)
            //{
            //    throw new Exception();
            //}

            var allClassTypes = GetAllConcreteClassTypes(typeof(TCommand));

            // TODO: only create with IAsyncCommand, if none then throw a handler not found exception

            var firstRegisteredClassType = allClassTypes.First(t => dependencyService.IsRegistered(typeof(IAsyncCommandHandler<,>).MakeGenericType(t, typeof(TSuccessResult))));

            var handlerType = typeof(IAsyncCommandHandler<,>).MakeGenericType(firstRegisteredClassType, typeof(TSuccessResult));
            var handler = dependencyService.Resolve(handlerType);
            var task = (Task<TSuccessResult>)handlerType.GetTypeInfo().GetMethod("ExecuteAsync", new[] { command.GetType() }).Invoke(handler, new object[] { command });
            return await task;
        }

        public async Task<CommandProcessorSuccessResult<TSuccessResult>> ProcessResultAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            try
            {
                return new CommandProcessorSuccessResult<TSuccessResult>(await ProcessAsync(command));
            }
            catch (Exception exception)
            {
                return new CommandProcessorSuccessResult<TSuccessResult>(exception);
            }
        }

        private static IEnumerable<Type> GetAllConcreteClassTypes(Type type)
        {
            var types = new List<Type>
            {
                type
            };
            while (type.GetTypeInfo().BaseType != null)
            {
                type = type.GetTypeInfo().BaseType;

                if (!type.GetTypeInfo().IsAbstract)
                {
                    types.Add(type);
                }
            }
            return types;
        }

        public void Dispose()
        {
            dependencyService.Dispose();
            Console.WriteLine("CommandProcessor disposed " + Id);
        }
    }
}
