namespace Octopost.Services.Posts
{
    using Octopost.Model.Data;
    using Octopost.Model.Dto;
    using Octopost.Model.Extensions;
    using Octopost.Services.Tagging;
    using Octopost.Services.UoW;
    using System;

    public class PostCreationService : IPostCreationService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IPostTaggingService postTaggingService;

        public PostCreationService(
            IUnitOfWorkFactory unitOfWorkFactory,
            IPostTaggingService postTaggingService)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.postTaggingService = postTaggingService;
        }

        public long CreatePost(CreatePostDto createPostDto)
        {
            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var repository = unitOfWork.CreateEntityRepository<Post>();
                var post = createPostDto.MapTo<Post>();
                post.Topic = this.PredictTopic(post.Text);
                repository.Create(post);
                unitOfWork.Save();
                return post.Id;
            }
        }

        private string PredictTopic(string text) =>
            this.postTaggingService.PredictTag(text);
    }
}
