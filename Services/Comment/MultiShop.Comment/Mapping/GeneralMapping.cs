using AutoMapper;

using MultiShop.Comment.DTOs;
using MultiShop.Comment.Entities;

namespace MultiShop.Catalog.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<UserComment, ResultCommentDto>().ReverseMap();
            CreateMap<UserComment, CreateCommentDto>().ReverseMap();
            CreateMap<UserComment, UpdateCommentDto>().ReverseMap();

          
        }
    }
}
