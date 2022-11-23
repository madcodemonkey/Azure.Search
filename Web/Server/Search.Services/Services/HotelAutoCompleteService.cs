using Search.CogServices;
using Search.Model;

namespace Search.Services;

public class HotelAutoCompleteService : AcmeAutoCompleteServiceBase, IHotelAutoCompleteService
{
    private readonly SearchServiceSettings _settings;

    /// <summary>Constructor</summary>
    public HotelAutoCompleteService(SearchServiceSettings settings,
        IAcmeSearchIndexService searchIndexService,
        IHotelFieldService fieldService) : base(searchIndexService, fieldService)
    {
        _settings = settings;
    }

    protected override string IndexName => _settings.Hotel.IndexName;
    protected override string SuggestorName => _settings.Hotel.SuggestorName;


    /// <summary>Gets a list of fields that we should be returned with the document found with the autocomplete.
    /// If left blank, it will return the key field. Warning! Field names must match exactly how they
    /// appear in the Azure Search Document!</summary>
    protected override List<string> GetFieldNamesToSearch()
    {
        return new List<string>
        {
            nameof(HotelDocument.HotelName).ConvertToCamelCase(),
            nameof(HotelDocument.Category).ConvertToCamelCase()
        };

    }
}