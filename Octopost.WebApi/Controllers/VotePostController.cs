namespace Octopost.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Octopost.Model.Data;
    using Octopost.Model.Dto;
    using Octopost.Model.Validation;
    using Octopost.Services.ApiResult;
    using Octopost.Services.Votes;
    using System;

    [Route("/api/Posts")]
    public class VotePostController : Controller
    {
        private readonly IVoteService voteService;

        private readonly IApiResultService apiResultService;

        public VotePostController(IVoteService voteService, IApiResultService apiResultService)
        {
            this.voteService = voteService;
            this.apiResultService = apiResultService;
        }

        [HttpPost("{postId}/Votes")]
        public IActionResult Vote(CreateVoteDto dto)
        {
            var state = (VoteState)Enum.Parse(typeof(VoteState), dto.VoteState);
            var created = this.voteService.Vote(dto.PostId, state);
            return this.apiResultService.Created(OctopostEntityName.Vote, created);
        }
    }
}
