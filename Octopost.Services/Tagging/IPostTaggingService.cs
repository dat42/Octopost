namespace Octopost.Services.Tagging
{
    public interface IPostTaggingService
    {
        string PredictTag(string text);
    }
}
