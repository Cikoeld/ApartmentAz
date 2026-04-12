using ApartmentAz.BLL.DTOs.Agency;
using ApartmentAz.BLL.DTOs.Listing;
using ApartmentAz.DAL.Models;
using AutoMapper;

namespace ApartmentAz.BLL.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Listing → CreateListingDto (reverse: DTO → Entity)
        CreateMap<CreateListingDto, Listing>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.City, opt => opt.Ignore())
            .ForMember(dest => dest.District, opt => opt.Ignore())
            .ForMember(dest => dest.Metro, opt => opt.Ignore())
            .ForMember(dest => dest.Agency, opt => opt.Ignore())
            .ForMember(dest => dest.ResidentialComplex, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.Translations, opt => opt.Ignore())
            .ForMember(dest => dest.CommercialDetail, opt => opt.Ignore());

        CreateMap<CreateListingTranslationDto, ListingTranslation>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ListingId, opt => opt.Ignore())
            .ForMember(dest => dest.Listing, opt => opt.Ignore());

        CreateMap<CreateCommercialDetailDto, CommercialDetail>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ListingId, opt => opt.Ignore())
            .ForMember(dest => dest.Listing, opt => opt.Ignore());

        // CommercialDetail → DTO
        CreateMap<CommercialDetail, ListingCommercialDetailDto>();

        // Agency
        CreateMap<CreateAgencyDto, Agency>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Listings, opt => opt.Ignore());
    }
}
