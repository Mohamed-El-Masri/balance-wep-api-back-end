using AutoMapper;
using balance.domain;
using balance.services.DTOs;

namespace balance.services.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Property, PropertyDTO>()
                .ForMember(dest => dest.PropertyTypeName, opt => opt.MapFrom(src => src.PropertyType != null ? src.PropertyType.Name : null))
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project != null ? src.Project.Name : null))
                .ForMember(dest => dest.AgentName, opt => opt.MapFrom(src => src.Agent != null ? src.Agent.UserName : null))
                .ForMember(dest => dest.OfferType, opt => opt.MapFrom(src => src.offer_type));
                
            CreateMap<PropertyDTO, Property>()
                .ForMember(dest => dest.offer_type, opt => opt.MapFrom(src => src.OfferType));

            CreateMap<PropertyType, PropertyTypeDTO>()
                .ForMember(dest => dest.PropertiesCount, opt => opt.MapFrom(src => src.Properties != null ? src.Properties.Count : 0));
                
            CreateMap<PropertyTypeDTO, PropertyType>();

            CreateMap<Project, ProjectDTO>()
                .ForMember(dest => dest.AgentName, opt => opt.MapFrom(src => src.Agent != null ? src.Agent.UserName : null))
                .ForMember(dest => dest.PropertiesCount, opt => opt.MapFrom(src => src.Properties != null ? src.Properties.Count : 0));
                
            CreateMap<ProjectDTO, Project>();

            CreateMap<Location, LocationDTO>();
            CreateMap<LocationDTO, Location>();

            CreateMap<GeoLocation, GeoLocationDTO>();
            CreateMap<GeoLocationDTO, GeoLocation>();

            CreateMap<PropertyImage, PropertyImageDTO>();
            CreateMap<PropertyImageDTO, PropertyImage>();

            CreateMap<PropertyFeature, PropertyFeatureDTO>()
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsActive));
                
            CreateMap<PropertyFeatureDTO, PropertyFeature>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsAvailable));

            CreateMap<Favorite, FavoriteDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null))
                .ForMember(dest => dest.PropertyTitle, opt => opt.MapFrom(src => src.Property != null ? src.Property.Title : null))
                .ForMember(dest => dest.PropertyPrice, opt => opt.MapFrom(src => src.Property != null ? src.Property.Price : 0))
                .ForMember(dest => dest.PropertyAddress, opt => opt.MapFrom(src => src.Property != null ? src.Property.Address : null))
                .ForMember(dest => dest.PropertyImageUrl, opt => 
                    opt.MapFrom(src => src.Property != null && src.Property.Images != null && src.Property.Images.Any() ? 
                        src.Property.Images.FirstOrDefault().ImageUrl : null))
                .ForMember(dest => dest.AddedDate, opt => opt.MapFrom(src => src.CreatedAt));
                
            CreateMap<FavoriteDTO, Favorite>();
        }
    }
}
