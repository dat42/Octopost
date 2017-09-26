namespace Octopost.Services.Exceptions
{
    using Octopost.Model.ApiResponse;
    using Octopost.Services.ApiResult;
    using System;

    public class ApiException : Exception
    {
        private static readonly IApiResultService apiResultService = new ApiResultService();

        public ApiException(Func<IApiResultService, IApiResult> apiResultCreator) =>
            this.ApiResult = apiResultCreator(ApiException.apiResultService);

        public IApiResult ApiResult { get; set; }
    }
}
