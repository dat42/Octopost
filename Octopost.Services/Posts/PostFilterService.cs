namespace Octopost.Services.Posts
{
    using Octopost.Model.Data;
    using Octopost.Model.Dto;
    using Octopost.Model.Extensions;
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
                return this.Filter(unitOfWork, queried, page, pageSize).OrderByDescending(x => x.Created);
            }
        }

        public IEnumerable<PostDto> FilterByTag(string[] tag, int page, int pageSize)
        {
            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var posts = unitOfWork.CreateEntityRepository<Post>()
                    .Query()
                    .Where(x => tag.Contains(x.Topic));
                return this.Filter(unitOfWork, posts, page, pageSize).OrderBy(x => x.VoteCount);
            }
        }

        public IEnumerable<PostDto> FilterByVotes(int page, int pageSize)
        {
            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var posts = unitOfWork.CreateEntityRepository<Post>().Query();
                return this.Filter(unitOfWork, posts, page, pageSize).OrderByDescending(x => x.VoteCount);
            }
        }

        private IEnumerable<PostDto> Filter(
            IUnitOfWork unitOfWork, 
            IQueryable<Post> posts, 
            int page, 
            int pageSize)
        {
            var paged = this.Page(posts, page, pageSize).AsEnumerable();
            foreach (var pagedItem in paged)
            {
                var mapped = pagedItem.MapTo<PostDto>();
                mapped.VoteCount = this.voteCountService.CountVotes(mapped.Id);
                yield return mapped;
            }
        }

        private IQueryable<T> Page<T>(IQueryable<T> queryable, int page, int pageSize) =>
            queryable.Skip(page * pageSize).Take(pageSize);
    }
}
