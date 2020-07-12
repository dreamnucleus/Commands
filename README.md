[![Build Status](https://mckendry.visualstudio.com/Commands/_apis/build/status/Commands)](https://mckendry.visualstudio.com/Commands/_build/latest?definitionId=8) [![NuGet](https://img.shields.io/nuget/v/DreamNucleus.Commands.svg)](https://www.nuget.org/packages/DreamNucleus.Commands/)


# Commands

The aim of this library to help in writing a business layer for applications. It includes a pipeline and notifcations (specific to a type of command) to wrap incoming commands, outgoing results and exceptions.

The base components are:

* Command
* Command Handler
* Command Processor
* Result Processor
* Executing Pipeline
* Executed Pipeline
* Exception Pipeline
* Executing Notification
* Executed Notification
* Exception Notification


Below is a simple example of executing the GetBlogCommand and processing the result

```cs

return await resultProcessor.For(new GetBlogCommand(blogId))
    .When(o => o.NotFound()).Return(r => new HttpResult(404))
    .When(o => o.Success()).Return(r => new HttpResult(200))
    .ExecuteAsync();
    
```

## NuGet

Packages availble for .NETFramework 4.5 and .NETStandard 1.6

https://www.nuget.org/packages/DreamNucleus.Commands/

```
Install-Package DreamNucleus.Commands
```
```
dotnet add package DreamNucleus.Commands
```


https://www.nuget.org/packages/DreamNucleus.Commands.Extensions/

```
Install-Package DreamNucleus.Commands.Extensions
```
```
dotnet add package DreamNucleus.Commands.Extensions
```


https://www.nuget.org/packages/DreamNucleus.Commands.Autofac/

```
Install-Package DreamNucleus.Commands.Autofac
```
```
dotnet add package DreamNucleus.Commands.Autofac
```


https://www.nuget.org/packages/DreamNucleus.Commands.Extensions.Redis/

```
Install-Package DreamNucleus.Commands.Extensions.Redis
```
```
dotnet add package DreamNucleus.Commands.Extensions.Redis
```

# Example

## Get Blog Command

```cs

// the interfaces let us know what will be returned or what exceptions can be thrown
public class GetBlogCommand : ISuccessResult<GetBlogCommand, BlogData>, INotFoundResult
{
    public int BlogId { get; }

    public GetBlogCommand(int blogId)
    {
        BlogId = blogId;
    }
}

public class BlogData
{
    public int BlogId { get; set; }
    public string Url { get; set; }
}

```


## Get Blog Command Handler

```cs

public class GetBlogCommandHandler : IAsyncCommandHandler<GetBlogCommand, BlogData>
{
    private readonly BloggingContext context;

    public GetBlogCommandHandler(BloggingContext context)
    {
        this.context = context;
    }

    public async Task<BlogData> ExecuteAsync(GetBlogCommand command)
    {
        var blog = await context.Blogs.SingleOrDefaultAsync(b => b.BlogId == command.BlogId);

        if (blog == null)
        {
            throw new NotFoundException();
        }

        return new BlogData
        {
            BlogId = blog.BlogId,
            Url = blog.Url
        };
    }
}

```


## Audit Log Pipeline

```cs
// this could be used to write all incoming commands to a audit log
public class AuditLogPipeline : Pipeline
{
    private readonly BloggingContext context;

    public AuditLogPipeline(BloggingContext context)
    {
        this.context = context;
    }
    
    public override Task ExecutingAsync(IAsyncCommand command)
    {
        // log the incoming command
        return base.ExecutingAsync(command);
    }

    public override async Task ExecutedAsync(IAsyncCommand command, object result)
    {
        // log the completed command and result
        return base.ExecutedAsync(command, result);
    }
    
    public override Task ExceptionAsync(IAsyncCommand command, Exception exception)
    {
        // log an exception in the command
        return base.ExceptionAsync(command, exception);
    }

}
```


## Executed Notification

```cs
// this could be used to send an email to the owner... maybe good if you get one visitor a day
public class ExecutedNotification : IExecutedNotification<GetBlogCommand, BlogData>
{
    public Task OnExecutedAsync(GetBlogCommand command, BlogData result)
    {
        // add email request to a queue
        return Task.FromResult(0);
    }
}
```


## Using Command Processor (with Autofac)

```cs

var containerBuilder = new ContainerBuilder();

containerBuilder.RegisterType<BloggingContext>().InstancePerLifetimeScope();

// these can be found and regsiters automatically
containerBuilder.RegisterType<GetBlogCommandHandler>().As<IAsyncCommandHandler<GetBlogCommand, BlogData>>();

var container = containerBuilder.Build();

var commandProcessor = new CommandProcessor(new LifetimeScopeService(container.BeginLifetimeScope()));

try
{
    var blog = await commandProcessor.ProcessAsync(new GetBlogCommand(1));
}
catch (Exception)
{
    // ignore
}

```


## Using Result Processor (with Autofac)

```cs

var containerBuilder = new ContainerBuilder();

containerBuilder.RegisterType<BloggingContext>().InstancePerLifetimeScope();

// these can be found and regsiters automatically
containerBuilder.RegisterType<GetBlogCommandHandler>().As<IAsyncCommandHandler<GetBlogCommand, BlogData>>();

var container = containerBuilder.Build();

// these are default handlers, local handlers are looked for first 
var resultRegister = new ResultRegister<HttpResult>();
resultRegister.When<NotFoundException>().Return(r => new HttpResult(404));

var resultProcessor = new ResultProcessor<HttpResult>(resultRegister.Emit(),
    new LifetimeScopeService(container.BeginLifetimeScope()));

// exceptions are caught and processed using the handlers
var result = await resultProcessor.For(new GetBlogCommand(1))
    .When(o => o.NotFound()).Return(r => new HttpResult(404))
    .When(o => o.Success()).Return(r => new HttpResult(200))
    .ExecuteAsync();

```

## Using Command Processor (with Autofac and Redis as transport)

```cs

var containerBuilder = new ContainerBuilder();

containerBuilder.RegisterType<BloggingContext>().InstancePerLifetimeScope();

// these can be found and regsiters automatically
containerBuilder.RegisterType<GetBlogCommandHandler>().As<IAsyncCommandHandler<GetBlogCommand, BlogData>>();

var container = containerBuilder.Build();

var commandProcessor = new CommandProcessor(new LifetimeScopeService(container.BeginLifetimeScope()));

var connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync("localhost");
var redisCommandTransportClient = new RedisCommandTransportClient(connectionMultiplexer, "commands", "results");
var redisCommandTransportServer = new RedisCommandTransportServer(connectionMultiplexer, "commands", "group", "consumer");

var commandProcessorClient = new CommandProcessorClient(redisCommandTransportClient);
var commandProcessorServer = new CommandProcessorServer(commandProcessor, redisCommandTransportServer);

await commandProcessorServer.StartAsync();

try
{
    // this command will be executed using redis as the transport between the client and server
    var blog = await commandProcessorClient.ProcessAsync(new GetBlogCommand(1));
}
catch (Exception)
{
    // ignore
}

await commandProcessorServer.StopAsync();

```
