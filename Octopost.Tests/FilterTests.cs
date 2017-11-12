namespace Octopost.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using Octopost.Model.Data;
    using Octopost.Model.Dto;
    using Octopost.Services.Posts;
    using Octopost.Services.UoW;
    using Octopost.Services.Votes;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    [TestClass]
    public class FilterTests : OctopostTestBase
    {
        [TestMethod]
        public async Task PostsByTags()
        {
            // Arrange
            const string writtenWorkTag = "WrittenWork";
            const string naturalPlaceTag = "NaturalPlace";
            const string companyTag = "Company";
            using (var unitOfWork = this.GetService<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                var postRepository = unitOfWork.CreateEntityRepository<Post>();
                for (var i = 0; i < 10; i++)
                {
                    postRepository.Create(new Post { Topic = writtenWorkTag, Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." });
                    postRepository.Create(new Post { Topic = naturalPlaceTag, Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." });
                    postRepository.Create(new Post { Topic = companyTag, Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." });
                }

                unitOfWork.Save();
            }

            const string url = "/api/Posts/Tags?page=0&pageSize=100";
            var writtenWork = string.Concat(url, "&tags=", writtenWorkTag);
            var naturalPlace = string.Concat(url, "&tags=", naturalPlaceTag);
            var both = string.Concat(url, "&tags=", writtenWorkTag, ",", naturalPlaceTag);
            var invalid = string.Concat(url, "&tags=Invalid");

            // Act
            var response = await this.Client.GetAsync(writtenWork);
            var content = await response.Content.ReadAsStringAsync();
            var writtenWorkPosts = JsonConvert.DeserializeObject<PostDto[]>(content);

            response = await this.Client.GetAsync(naturalPlace);
            content = await response.Content.ReadAsStringAsync();
            var naturalPlacePosts = JsonConvert.DeserializeObject<PostDto[]>(content);

            response = await this.Client.GetAsync(both);
            content = await response.Content.ReadAsStringAsync();
            var bothPosts = JsonConvert.DeserializeObject<PostDto[]>(content);

            response = await this.Client.GetAsync(invalid);
            content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.IsTrue(writtenWorkPosts.All(x => x.Topic == writtenWorkTag));
            Assert.IsTrue(naturalPlacePosts.All(x => x.Topic == naturalPlaceTag));
            Assert.IsTrue(writtenWorkPosts.All(x => x.Topic == writtenWorkTag));
            Assert.IsTrue(bothPosts.All(x => x.Topic == writtenWorkTag || x.Topic == naturalPlaceTag));
            Assert.IsTrue(content.Contains("INVALID_TAG_ID"));
        }

        [TestMethod]
        public async Task PostsByVotes()
        {
            // Arrange
            const string url = "/api/Posts/Votes?page=0&pageSize=100";
            var postCreationService = this.GetService<IPostCreationService>();
            var postVoteService = this.GetService<IVoteService>();
            var firstId = postCreationService.CreatePost(new CreatePostDto { Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." });
            var secondId = postCreationService.CreatePost(new CreatePostDto { Text = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." });
            postVoteService.Vote(firstId, VoteState.Up);
            postVoteService.Vote(firstId, VoteState.Down);
            postVoteService.Vote(firstId, VoteState.Up);
            postVoteService.Vote(secondId, VoteState.Down);
            postVoteService.Vote(secondId, VoteState.Down);
            postVoteService.Vote(secondId, VoteState.Down);

            // Act
            var response = await this.Client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var posts = JsonConvert.DeserializeObject<PostDto[]>(content);

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(posts.Length, 2);
            Assert.AreEqual(posts[0].Id, firstId);
            Assert.AreEqual(posts[1].Id, secondId);
            Assert.AreEqual(posts[0].VoteCount, 1);
            Assert.AreEqual(posts[1].VoteCount, -3);
        }

        [TestMethod]
        public async Task PostsByNewest()
        {
            // Arrange
            const string url = "/api/Posts/Newest?page=0&pageSize=100";
            var postCreationService = this.GetService<IPostCreationService>();
            var firstid = postCreationService.CreatePost(new CreatePostDto { Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." });
            var secondId = postCreationService.CreatePost(new CreatePostDto { Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." });

            // Act
            var response = await this.Client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var posts = JsonConvert.DeserializeObject<PostDto[]>(content);

            // Assert
            Assert.IsTrue(posts[0].Created > posts[1].Created);
        }
    }
}
