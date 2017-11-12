namespace Octopost.Services.Posts
{
    using Octopost.Model.Data;
    using Octopost.Model.Dto;
    using Octopost.Model.Extensions;
    using Octopost.Model.Settings;
    using Octopost.Services.Tagging;
    using Octopost.Services.UoW;
    using System;

    public class PostCreationService : IPostCreationService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IPostTaggingService postTaggingService;

        private readonly OctopostSettings settings;

        public PostCreationService(
            IUnitOfWorkFactory unitOfWorkFactory,
            IPostTaggingService postTaggingService,
            OctopostSettings settings)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.postTaggingService = postTaggingService;
            this.settings = settings;
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
