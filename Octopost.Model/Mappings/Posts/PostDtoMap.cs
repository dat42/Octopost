namespace Octopost.Model.Mappings.Posts
{
    using AutoMapper;
    using Octopost.Model.Data;
    using Octopost.Model.Dto;

    public class PostDtoMap : Profile
    {
        public PostDtoMap()
        {
            this.CreateMap<Post, PostDto>()
                .ForMember(x => x.Created, x => x.MapFrom(y => y.Created))
                .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
                .ForMember(x => x.Text, x => x.MapFrom(y => y.Text))
                .ForMember(x => x.Topic, x => x.MapFrom(y => y.Topic))
                .ForMember(x => x.VoteCount, x => x.Ignore())
                .ReverseMap();
        }
    }
}
