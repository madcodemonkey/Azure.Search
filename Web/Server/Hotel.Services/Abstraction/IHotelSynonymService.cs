namespace Hotel.Services;

public interface IHotelSynonymService
{
    Task<bool> AssociateSynonymMapToHotelFieldsAsync(string hotelIndexName, string synonymMapName);
}