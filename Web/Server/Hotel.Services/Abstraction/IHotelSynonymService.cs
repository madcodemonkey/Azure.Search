namespace Hotel.Services;

public interface IHotelSynonymService : ISearchSynonymService
{
    /// <summary>Creates all the synonym lists used by the hotel index.</summary>
    Task<string> CreateAsync();

    Task<bool> AssociateSynonymMapToHotelFieldsAsync(string hotelIndexName, string synonymMapName);
}