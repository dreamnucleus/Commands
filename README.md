# Commands

The aim of this library to help in writing Web API's. In a controller the code will look like the below. There is also the ability to use default handlers for errors such as not found.

```cs

return await resultProcessor.For(new GetBlogCommand(blogId))
    .When(o => o.NotFound()).Return(r => new HttpResult(404))
    .When(o => o.Success()).Return(r => new HttpResult(200))
    .ExecuteAsync();
    
```

## NuGet

https://www.nuget.org/packages/Slipstream.CommonDotNet.Commands/

https://www.nuget.org/packages/Slipstream.CommonDotNet.Commands.Autofac/


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


