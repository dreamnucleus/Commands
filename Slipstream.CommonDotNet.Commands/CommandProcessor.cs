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
#if DEBUG
        public Guid Id { get; } = Guid.NewGuid();
#endif

        private readonly ILifetimeScopeDependencyService dependencyService;
        public CommandProcessor(ILifetimeScopeDependencyService dependencyService)
        {
            this.dependencyService = dependencyService;
            //Console.WriteLine("CommandProcessor created: " + Id);
            //this.dependencyService = lifetimeScopeService.BeginLifetimeScope(this);
        }

        public async Task<IResult> ProcessAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
        {
            //var validationContext = new ValidationContext(command);
            //var validationResults = new List<ValidationResult>();

            //var isValid = Validator.TryValidateObject(command, validationContext, validationResults, true);

            //if (!isValid)
            //{
            //    throw new Exception();
            //}

            var allClassTypes = GetAllConcreteClassTypes(typeof(TCommand));
            var firstRegisteredClassType = allClassTypes.First(t => dependencyService.IsRegistered(typeof(IAsyncCommandHandler<>).MakeGenericType(t)));

            var handlerType = typeof(IAsyncCommandHandler<>).MakeGenericType(firstRegisteredClassType);
            var handler = dependencyService.Resolve(handlerType);
            var task = (Task<IResult>)handlerType.GetTypeInfo().GetMethod("ExecuteAsync", new[] { command.GetType() }).Invoke(handler, new object[] { command });
            return await task;
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

    }
}
