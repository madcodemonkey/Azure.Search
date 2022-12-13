using AutoMapper;
using Azure.Search.Documents.Models;
using Search.CogServices;
using Search.Model;
using Search.Web.Models;

namespace Search.Web.Configuration;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateAutoCompleteMappings();
        CreateSuggestionMappings();
        CreateSearchMappings();

        CreateMap<AcmeSearchQueryDto, AcmeSearchQuery>()
            .ForMember(m => m.QueryType, opt => opt.MapFrom(mf => mf.UseSemanticSearch ? SearchQueryType.Semantic : SearchQueryType.Simple));
    }

    private void CreateAutoCompleteMappings()
    {
        CreateMap<AutocompleteItem, HotelAutocompleteDto>();
    }

    private void CreateSearchMappings()
    {
        CreateMap<SearchResult<HotelDocument>, HotelDocumentDto>()
            .ForMember(m => m.HotelId, opt => opt.MapFrom(m => m.Document.HotelId))
            .ForMember(m => m.BaseRate, opt => opt.MapFrom(m => m.Document.BaseRate))
            .ForMember(m => m.Description, opt => opt.MapFrom(m => m.Document.Description))
            .ForMember(m => m.DescriptionFr, opt => opt.MapFrom(m => m.Document.DescriptionFr))
            .ForMember(m => m.HotelName, opt => opt.MapFrom(m => m.Document.HotelName))
            .ForMember(m => m.Category, opt => opt.MapFrom(m => m.Document.Category))
            .ForMember(m => m.Tags, opt => opt.MapFrom(m => m.Document.Tags))
            .ForMember(m => m.ParkingIncluded, opt => opt.MapFrom(m => m.Document.ParkingIncluded))
            .ForMember(m => m.SmokingAllowed, opt => opt.MapFrom(m => m.Document.SmokingAllowed))
            .ForMember(m => m.LastRenovationDate, opt => opt.MapFrom(m => m.Document.LastRenovationDate))
            .ForMember(m => m.Rating, opt => opt.MapFrom(m => m.Document.Rating))
            .ForMember(m => m.Location, opt => opt.MapFrom(m => m.Document.Location));
        CreateMap<AcmeSearchQueryResult<SearchResult<HotelDocument>>, AcmeSearchQueryResult<HotelDocumentDto>>();
    }

    private void CreateSuggestionMappings()
    {
        CreateMap<SearchSuggestion<HotelDocument>, HotelSuggestorDto>()
            .ForMember(m => m.Category, opt => opt.MapFrom(m => m.Document.Category))
            .ForMember(m => m.HotelName, opt => opt.MapFrom(m => m.Document.HotelName));
    }
}