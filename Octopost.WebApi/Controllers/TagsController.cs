namespace Octopost.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Octopost.Services.ApiResult;
    using Octopost.Services.Tagging;

    [Route("api/[controller]")]
    public class TagsController : Controller
    {
        private readonly IPostTaggingService postTaggingService;

        private readonly IApiResultService apiResultService;

        public TagsController(IPostTaggingService postTaggingService, IApiResultService apiResultService)
        {
            this.apiResultService = apiResultService;
            this.postTaggingService = postTaggingService;
        }

        [HttpGet]
        public IActionResult GetTags()
        {
            var tags = this.postTaggingService.GetTags();
            return this.apiResultService.Ok(tags);
        }

        [HttpPost]
        public IActionResult PredictTag([FromQuery]string text)
        {
            var prediction = this.postTaggingService.PredictTag(text);
            return this.apiResultService.Ok(prediction);
        }
    }
}
