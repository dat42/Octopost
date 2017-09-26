namespace Octopost.Model.Data
{
    using Octopost.Model.Interfaces;
    using System;

    public class Post : IIdentifiable, ICreated
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public string Topic { get; set; }

        public DateTime Created { get; set; }
    }
}
