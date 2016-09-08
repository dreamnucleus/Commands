using System;
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

        public async Task<IResult> ProcessAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
            where TSuccessResult : IResult
        {
            //var validationContext = new ValidationContext(command);
            //var validationResults = new List<ValidationResult>();

            //var isValid = Validator.TryValidateObject(command, validationContext, validationResults, true);

            //if (!isValid)
            //{
            //    throw new Exception();
            //}

            var allClassTypes = GetAllConcreteClassTypes(typeof(TCommand));

            // TODO: only create with IAsyncCommand, if none then throw a handler not found exception

            var firstRegisteredClassType = allClassTypes.First(t => dependencyService.IsRegistered(typeof(IAsyncCommandHandler<>).MakeGenericType(t)));

            var handlerType = typeof(IAsyncCommandHandler<>).MakeGenericType(firstRegisteredClassType);
            var handler = dependencyService.Resolve(handlerType);
            var task = (Task<IResult>)handlerType.GetTypeInfo().GetMethod("ExecuteAsync", new[] { command.GetType() }).Invoke(handler, new object[] { command });
            return await task;
        }

        public async Task<CommandProcessorSuccessResult<TSuccessResult>> ProcessSuccessAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
            where TSuccessResult : IResult
        {
            return new CommandProcessorSuccessResult<TSuccessResult>(await ProcessAsync(command));
        }

        private IEnumerable<Type> GetAllConcreteClassTypes(Type type)
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
