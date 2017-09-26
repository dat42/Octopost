namespace Octopost.Model.Validation
{
    using System;

    public class PropertyName : IEquatable<PropertyName>
    {
        internal PropertyName(string propertyName) =>
            this.Name = propertyName;

        public string Name { get; }

        public static readonly PropertyName Empty = new PropertyName("NONE");

        public static implicit operator string(PropertyName name) =>
            name.Name;

        public static explicit operator PropertyName(string name) =>
            new PropertyName(name);

        public static bool operator ==(PropertyName left, PropertyName right) =>
            left.Name == right.Name;

        public static bool operator !=(PropertyName left, PropertyName right) =>
            left.Name != right.Name;

        public static PropertyName Parse(string name) =>
            new PropertyName(name);

        public bool Equals(PropertyName other) =>
            this.Name == other.Name;

        public override int GetHashCode() =>
            this.Name.GetHashCode();

        public override bool Equals(object obj) =>
            obj is PropertyName propertyName && propertyName.Name == this.Name;

        public static class Post
        {
            public static readonly PropertyName Id = new PropertyName("ID");

            public static readonly PropertyName Text = new PropertyName("TEXT");

            public static readonly PropertyName Topic = new PropertyName("TAG");
        }

        public static class Vote
        {
            public static readonly PropertyName Id = new PropertyName("ID");

            public static readonly PropertyName VoteState = new PropertyName("STATE");
        }
    }
}
