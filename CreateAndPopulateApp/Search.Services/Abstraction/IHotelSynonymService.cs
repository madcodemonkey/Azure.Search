namespace Search.Services;

public interface IHotelSynonymService
{
    Task<bool> AssociateSynonymMapToHotelFieldsAsync(string hotelIndexName, string synonymMapName);
}