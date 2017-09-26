namespace Octopost.Services.Posts
{
    using Octopost.Model.Data;
    using Octopost.Services.UoW;
    using System.Linq;

    public class VoteCountService : IVoteCountService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public VoteCountService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public long CountVotes(long postId)
        {
            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var voteRepository = unitOfWork.CreateEntityRepository<Vote>();
                var voteCount = voteRepository.Query().Where(x => x.PostId == postId)
                    .Aggregate(0, (current, vote) => vote.State == VoteState.Down ? current - 1 : current + 1);
                return voteCount;
            }
        }
    }
}
