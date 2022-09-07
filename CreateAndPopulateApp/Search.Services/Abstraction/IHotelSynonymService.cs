namespace Search.Services;

public interface IHotelSynonymService
{
    Task<bool> AssociateSynonymMapToFieldsAsync(string hotelIndexName, string synonymMapName);
}