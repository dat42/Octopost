namespace Octopost.Services.Tagging
{
    using Newtonsoft.Json;
    using Octopost.Model.Dto.Tagging;
    using Octopost.Model.Settings;
    using System.Collections.Generic;
    using System.Net;
    using System.Text.Encodings.Web;

    public class PostTaggingService : IPostTaggingService
    {
        private readonly OctopostSettings octopostSettings;

        private Dictionary<long, string> classes;

        private WebClient webClient = new WebClient();

        public PostTaggingService(OctopostSettings octopostSettings)
        {
            this.octopostSettings = octopostSettings;
            this.FillClasses();
        }

        public string PredictTag(string text)
        {
            var encodedQuery = UrlEncoder.Default.Encode(text);
            var url = this.octopostSettings.TopicClassifierUrl + "/prediction?text=" + encodedQuery;
            var result = this.webClient.DownloadString(url);
            var prediction = JsonConvert.DeserializeObject<Prediction>(result);
            return prediction.Text;
        }

        public IDictionary<long, string> GetTags()
        {
            return this.classes;
        }

        private void FillClasses()
        {
            if (this.classes == null)
            {
                var url = this.octopostSettings.TopicClassifierUrl + "/classes";
                var result = this.webClient.DownloadString(url);
                var parsed = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                this.classes = new Dictionary<long, string>();
                foreach (var item in parsed)
                {
                    this.classes.Add(long.Parse(item.Key), item.Value);
                }
            }
        }
    }
}
