using AutoMapper;
using CodecoolApi.DAL.DTO.Author;
using CodecoolApi.DAL.DTO.Material;
using CodecoolApi.DAL.DTO.MaterialType;
using CodecoolApi.DAL.DTO.Review;
using CodecoolApi.Models;

namespace CodecoolApi.Mapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<PostAuthorDto, Author>();
            CreateMap<PutAuthorDto, Author>();
            CreateMap<Author, GetAuthorDto>()
                .ForMember(x => x.Materials, opt => opt.MapFrom(src => src.Materials.Select(x => x.Title)))
                .ForMember(x => x.NumbersOfMaterials, opt => opt.MapFrom(src => src.Materials.Count()));
            CreateMap<PostMaterialTypeDto, MaterialType>();
            CreateMap<PostMaterialTypeDto, Material>();
            CreateMap<PostReviewDto, Review>();
            CreateMap<MaterialType, GetMaterialTypeDto>()
                .ForMember(x => x.Materials, opt => opt.MapFrom(src => src.Materials.Select(x => x.Title)));
            CreateMap<Review, GetReviewDto>()
                .ForMember(x => x.Material, opt => opt.MapFrom(src => src.Material.Title));
            CreateMap<Material, GetMaterialDto>()
            .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews.Select(x => x.Text)))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.UserName))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.Name));
        }
    }
}
