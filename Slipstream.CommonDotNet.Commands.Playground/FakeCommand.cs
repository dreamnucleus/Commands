using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands.Playground
{
    public class FakeCommand : ISuccessResult<FakeCommand, FakeData>, INotFoundResult, IConflictResult
    {
        public FakeCommand(int number)
        {
            Number = number;
        }

        public int Number { get; set; }
    }

    public class FakeData
    {
        public int Id { get; set; }
    }

    public class FakeCommandHandler : IAsyncCommandHandler<FakeCommand, FakeData>
    {
        private readonly ICommandProcessor commandProcessor;

        public FakeCommandHandler(ICommandProcessor commandProcessor)
        {
            this.commandProcessor = commandProcessor;
        }


        public bool ToReturn { get; set; }

        public async Task<FakeData> ExecuteAsync(FakeCommand command)
        {
            if (command.Number == -1)
            {
                throw new Exception("Test");
            }

            await Task.Delay(1);

            return new FakeData();

            //var toReturn = "";

            //if (command.Number == 0)
            //{
            //}
            //else if (command.Number == 1)
            //{
            //    return 2;
            //}
            //else if (command.Number == -1)
            //{
            //    return TestException.Create();
            //}
            //else if (command.Number == 2)
            //{
            //    return toReturn;
            //}
            //else if (command.Number == -2)
            //{
            //    return ToReturn;
            //    return this.ToReturn;

            //}
            //else if (command.Number == 3)
            //{
            //    //  SimpleMemberAccessExpression
            //    return new Testing().ConsoleColor;
            //}
            //else if (command.Number == -3)
            //{
            //    return new Testing().DayOfWeek();
            //}
            //else if (command.Number == 4)
            //{
            //    //  SimpleMemberAccessExpression
            //    return (object)"";
            
            //}
            //else if (command.Number == -4)
            //{
            //    return "" as object;
            //}
            //else if (command.Number == 5)
            //{
            //    return "" == "";
            //}
            //else if (command.Number == -5)
            //{
            //    return "" != "";
            //}
            //else if (command.Number == 6)
            //{
            //    return Test() ?? new NotFoundException();
            //}
            //else if (command.Number == -6)
            //{
            //    var getBlogResult = await commandProcessor.ProcessResultAsync(new GetBlogCommand(1));
            //    if (getBlogResult.NotSuccess)
            //    {
            //        return getBlogResult.Result;
            //    }
            //    else if (command.Number == 213414)
            //    {
            //        return Test();
            //    }
            //    else
            //    {
            //        return getBlogResult.SuccessResult;
            //    }
            //}
            //else
            //{
            //    return command.Number == 2 ? Test() : new NotFoundException();
            //}

            
            // ??
        }


        private object Test()
        {
            return new ConflictException();
        }
    }

    public class Testing
    {
        public ConsoleColor ConsoleColor { get; set; } = ConsoleColor.Black;

        public DayOfWeek DayOfWeek()
        {
            return System.DayOfWeek.Friday;
        }
    }
}
