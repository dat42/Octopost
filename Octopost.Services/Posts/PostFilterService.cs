namespace Octopost.Services.Posts
{
    using Octopost.Model.ApiResponse.HTTP400;
    using Octopost.Model.Data;
    using Octopost.Model.Dto;
    using Octopost.Model.Extensions;
    using Octopost.Model.Validation;
    using Octopost.Services.Exceptions;
    using Octopost.Services.UoW;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PostFilterService : IPostFilterService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IVoteCountService voteCountService;

        public PostFilterService(IUnitOfWorkFactory unitOfWorkFactory, IVoteCountService voteCountService)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.voteCountService = voteCountService;
        }

        public IEnumerable<PostDto> FilterByDate(DateTime from, DateTime to, int page, int pageSize)
        {
            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var postRepository = unitOfWork.CreateEntityRepository<Post>();
                var queried = postRepository.Query().Where(x => x.Created >= from && x.Created <= to).OrderBy(x => x.Created);
                var filtered = this.Filter(queried, page, pageSize)
                    .OrderByDescending(x => x.Created)
                    .ThenByDescending(x => x.VoteCount);
                return filtered;
            }
        }

        public IEnumerable<PostDto> FilterByTag(string[] tag, int page, int pageSize)
        {
            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var posts = unitOfWork.CreateEntityRepository<Post>()
                    .Query()
                    .Where(x => tag.Contains(x.Topic));
                var filtered = this.Filter(posts, page, pageSize)
                    .OrderByDescending(x => x.VoteCount)
                    .ThenByDescending(x => x.Created);
                return filtered;
            }
        }

        public IEnumerable<PostDto> FilterByVotes(int page, int pageSize)
        {
            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var posts = unitOfWork.CreateEntityRepository<Post>().Query();
                var filtered = this.Filter(posts, page, pageSize)
                    .OrderByDescending(x => x.VoteCount)
                    .ThenByDescending(x => x.Created);
                return filtered;
            }
        }

        private PostDto[] Filter(
            IQueryable<Post> posts, 
            int page, 
            int pageSize)
        {
            if (pageSize < 1)
            {
                throw new ApiException(x => x.BadRequestResult(
                    (ErrorCode.Parse(ErrorCodeType.OutOfRange, OctopostEntityName.Filter, PropertyName.Filter.PageSize),
                    new ErrorDefinition(pageSize, "Page size must be 1 or bigger", PropertyName.Filter.PageSize))));
            }

            if (page < 0)
            {
                throw new ApiException(x => x.BadRequestResult(
                    (ErrorCode.Parse(ErrorCodeType.OutOfRange, OctopostEntityName.Filter, PropertyName.Filter.PageNumber),
                    new ErrorDefinition(page, "Page number cannot be negative", PropertyName.Filter.PageNumber))));
            }

            var paged = this.Page(posts, page, pageSize).ToArray();
            var returnArray = new PostDto[paged.Length];
            for (var i = 0; i < paged.Length; i++)
            {
                var mapped = paged[i].MapTo<PostDto>();
                mapped.VoteCount = this.voteCountService.CountVotes(mapped.Id);
                returnArray[i] = mapped;
            }

            return returnArray;
        }

        private IQueryable<T> Page<T>(IQueryable<T> queryable, int page, int pageSize) =>
            queryable.Skip(page * pageSize).Take(pageSize);
    }
}
