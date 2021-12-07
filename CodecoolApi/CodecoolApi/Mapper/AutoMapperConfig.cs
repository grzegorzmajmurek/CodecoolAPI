using AutoMapper;
using CodecoolApi.DAL.DTO.Author;
using CodecoolApi.DAL.DTO.MaterialType;
using CodecoolApi.DAL.DTO.Review;
using CodecoolApi.Models;

namespace CodecoolApi.Mapper
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PostAuthorDto, Author>();
                cfg.CreateMap<PutAuthorDto, Author>();
                cfg.CreateMap<PostMaterialTypeDto, MaterialType>();
                cfg.CreateMap<PostMaterialTypeDto, Material>();
                cfg.CreateMap<PostReviewDto, Review>();
            })
        .CreateMapper();
    }
}
