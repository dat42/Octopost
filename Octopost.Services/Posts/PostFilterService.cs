namespace Octopost.Services.Posts
{
    using Octopost.Model.ApiResponse.HTTP400;
    using Octopost.Model.Data;
    using Octopost.Model.Dto;
    using Octopost.Model.Validation;
    using Octopost.Services.Exceptions;
    using Octopost.Services.Tagging;
    using Octopost.Services.UoW;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PostFilterService : IPostFilterService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IVoteCountService voteCountService;

        private readonly IPostTaggingService postTaggingService;

        public PostFilterService(IUnitOfWorkFactory unitOfWorkFactory, IVoteCountService voteCountService, IPostTaggingService postTaggingService)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.voteCountService = voteCountService;
            this.postTaggingService = postTaggingService;
        }

        public IEnumerable<PostDto> FilterByDate(DateTime from, DateTime to, int page, int pageSize)
        {
            this.ThrowIfPageOutOfRange(page, pageSize);
            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var posts = this.QueryPostDto(unitOfWork);
                var queried = posts
                    .Where(x => x.Created >= from && x.Created <= to)
                    .OrderByDescending(x => x.Created)
                    .ThenByDescending(x => x.VoteCount);
                var filtered = this.Page(queried, page, pageSize);
                return filtered.ToList();
            }
        }

        public IEnumerable<PostDto> FilterByTag(string[] tags, int page, int pageSize)
        {
            this.ThrowIfPageOutOfRange(page, pageSize);
            var classes = this.postTaggingService.GetTags();
            var notFound = new List<string>();
            foreach (var tag in tags)
            {
                if (classes.Values.Contains(tag))
                {
                    notFound.Add(tag);
                }
            }

            if (notFound.Any())
            {
                var attemptedValues = string.Join(", ", tags);
                var notFoundValues = string.Join(", ", notFound);
                throw new ApiException(x => x.BadRequestResult(
                    (ErrorCode.Parse(ErrorCodeType.InvalidReferenceId, OctopostEntityName.Tag, PropertyName.Tag.TagName),
                    new ErrorDefinition(attemptedValues, $"The following tags do not exist: {notFoundValues}", PropertyName.Tag.TagName))));
            }

            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var posts = this.QueryPostDto(unitOfWork)
                    .Where(x => tags.Contains(x.Topic))
                    .OrderByDescending(x => x.VoteCount)
                    .ThenByDescending(x => x.Created);
                var filtered = this.Page(posts, page, pageSize);
                return filtered.ToList();
            }
        }

        public IEnumerable<PostDto> FilterByVotes(int page, int pageSize)
        {
            this.ThrowIfPageOutOfRange(page, pageSize);
            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var fetched = this.QueryPostDto(unitOfWork)
                    .OrderByDescending(x => x.VoteCount)
                    .ThenByDescending(x => x.Created);
                var postsPage = this.Page(fetched, page, pageSize);
                return postsPage.ToList();
            }
        }

        private IQueryable<PostDto> QueryPostDto(IUnitOfWork unitOfWork)
        {
            var fetched =
                from post in unitOfWork.CreateEntityRepository<Post>().Query()
                join vote in unitOfWork.CreateEntityRepository<Vote>().Query() on post.Id equals vote.PostId into posts
                from postWithVote in posts.DefaultIfEmpty(new Vote { State = VoteState.Neutral })
                group postWithVote by new { post.Id, post.Text, post.Topic, post.Created } into grouped
                select new PostDto
                {
                    Created = grouped.Key.Created,
                    Text = grouped.Key.Text,
                    Id = grouped.Key.Id,
                    Topic = grouped.Key.Topic,
                    VoteCount = grouped.Sum(x => (int)x.State)
                };
            return fetched;
        }

        private IQueryable<T> Page<T>(IQueryable<T> queryable, int page, int pageSize) =>
            queryable.Skip(page * pageSize).Take(pageSize);

        private void ThrowIfPageOutOfRange(int pageSize, int page)
        {
            if (pageSize < 0)
            {
                throw new ApiException(x => x.BadRequestResult(
                    (ErrorCode.Parse(ErrorCodeType.OutOfRange, OctopostEntityName.Filter, PropertyName.Filter.PageSize),
                    new ErrorDefinition(pageSize, "Page size must be 0 or bigger", PropertyName.Filter.PageSize))));
            }

            if (page < 0)
            {
                throw new ApiException(x => x.BadRequestResult(
                    (ErrorCode.Parse(ErrorCodeType.OutOfRange, OctopostEntityName.Filter, PropertyName.Filter.PageNumber),
                    new ErrorDefinition(page, "Page number cannot be negative", PropertyName.Filter.PageNumber))));
            }
        }
    }
}
