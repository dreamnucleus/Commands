using Microsoft.EntityFrameworkCore;
using Slipstream.CommonDotNet.Commands.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands.Playground
{
    public class TestCommand : ISuccessResult<TestCommand, List<TestData>>, INotFoundResult, IConflictResult
    {
    }

    public class TestCommandHandler : IAsyncCommandHandler<TestCommand, List<TestData>>
    {
        private readonly BloggingContext context;

        public TestCommandHandler(BloggingContext context)
        {
            this.context = context;
        }

        public Task<List<TestData>> ExecuteAsync(TestCommand command)
        {

            new List<int>().Single();

            //if (new Random().Next(0, 20) > 15)
            //{
            //    throw new NotFoundException();
            //}
            //else
            {
                return Task.FromResult(new List<TestData>
                {
                    new TestData()
                });
            }
        }
    }

    public class TestData
    {
        public int Code { get; set; } = 200;
    }


    public class GetBlogCommand : ISuccessResult<GetBlogCommand, BlogData>, INotFoundResult
    {
        public int BlogId { get; set; }

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

    public class CreatePostCommand : ISuccessResult<CreatePostCommand, PostData>, INotFoundResult, IConflictResult
    {
        public int BlogId { get; set; }

        public string PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public CreatePostCommand(int blogId, string postId, string title, string content)
        {
            BlogId = blogId;
            PostId = postId;
            Title = title;
            Content = content;
        }
    }

    public class PostData
    {
        public string PostId { get; set; }
    }


    public class CreatePostCommandHandler : IAsyncCommandHandler<CreatePostCommand, PostData>
    {
        private readonly ICommandProcessor commandProcessor;
        private readonly BloggingContext context;

        public CreatePostCommandHandler(ICommandProcessor commandProcessor, BloggingContext context)
        {
            this.commandProcessor = commandProcessor;
            this.context = context;
        }

        public async Task<PostData> ExecuteAsync(CreatePostCommand command)
        {
            var getBlogResult = await commandProcessor.ProcessResultAsync(new GetBlogCommand(command.BlogId));
            if (!getBlogResult.Success)
            {
                throw getBlogResult.Exception;
            }

            if (await context.Posts.AnyAsync(b => b.PostId == command.PostId))
            {
                throw new ConflictException();
            }

            context.Posts.Add(new Post
            {
                BlogId = command.BlogId,
                PostId = command.PostId,
                Title = command.Title,
                Content = command.Content
            });

            await context.SaveChangesAsync();

            // return await Processor.For(new GetBlogCommand(command.BlogId))
            //      .When(o => o.NotSuccess()).Return(r => r)
            //      .When(o => o.Success()).Return(r => {
            //          
            //      })

            return new PostData
            {
                PostId = command.PostId
            };
        }
    }
}
