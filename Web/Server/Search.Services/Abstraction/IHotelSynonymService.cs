namespace Search.Services;

public interface IHotelSynonymService : ISearchSynonymService
{
    /// <summary>Creates all the synonym lists used by the hotel index.</summary>
    Task<string> CreateAsync();

    /// <summary>Deletes hotel synonym map</summary>
    Task<bool> DeleteAsync();
    
    /// <summary>Associates a synonym map with certain fields on the Hotel Index</summary>
    Task AssociateSynonymMapToHotelFieldsAsync();
}