using AutoMapper;
using ProductSIMService.Dtos.Queries;
using ProductSIMService.Model;
using System.Collections.Generic;

namespace ProductSIMService.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Source -> Target
            CreateMap<Choice, ChoiceDto>();
            CreateMap<Cover, CoverDto>();
            CreateMap<Question, QuestionDto>();
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => src.ProductIcon))
                .ForMember(dest => dest.Covers, opt => opt.MapFrom(src => src.Covers))
                .ForMember(dest => dest.Questions, opt => opt.Ignore());

        }
    }
}