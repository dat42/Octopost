namespace Octopost.Model.Mappings.Posts
{
    using AutoMapper;
    using Octopost.Model.Data;
    using Octopost.Model.Dto;

    public class CreatePostDtoMap : Profile
    {
        public CreatePostDtoMap()
        {
            this.CreateMap<CreatePostDto, Post>()
                .ForMember(x => x.Created, x => x.Ignore())
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.Text, x => x.MapFrom(p => p.Text))
                .ForMember(x => x.Topic, x => x.Ignore())
                .ForMember(x => x.Votes, x => x.Ignore());
        }
    }
}
