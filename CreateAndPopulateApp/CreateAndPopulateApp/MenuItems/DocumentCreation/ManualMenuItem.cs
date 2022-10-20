using ConsoleMenuHelper;
using Microsoft.Spatial;
using Search.Model;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("DocumentCreation", 1)]
public class ManualMenuItem : IConsoleMenuItem
{
    private readonly SearchServiceSettings _settings;
    private readonly IPromptHelper _promptHelper;
    private readonly IAcmeSearchIndexService _indexService;

    /// <summary>Constructor</summary>
    public ManualMenuItem(SearchServiceSettings settings, IPromptHelper promptHelper, IAcmeSearchIndexService indexService)
    {
        _settings = settings;
        _promptHelper = promptHelper;
        _indexService = indexService;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        string hotelIndexName = _promptHelper.GetText($"Name of the HOTEL index (Default: {_settings.HotelIndexName})?", true, true);
        if (string.IsNullOrWhiteSpace(hotelIndexName))
            hotelIndexName = _settings.HotelIndexName;

        if (hotelIndexName != "exit")
        {
            var uploadList = GetHotels();

            await _indexService.UploadDocuments(hotelIndexName, uploadList);
        }

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Manually create documents via code";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;


    private List<Hotel> GetHotels()
    {
        var result = new List<Hotel>
        {
            new()
            {
                HotelId = Guid.NewGuid().ToString(),
                BaseRate = 325.00,
                HotelName = "Secret Point Motel",
                Description = "The hotel is ideally located on the main commercial artery of the city in the heart of New York. A few minutes away is Time's Square and the historic centre of the city, as well as other places of interest that make New York one of America's most attractive and cosmopolitan cities.",
                DescriptionFr = "L'hôtel est idéalement situé sur la principale artère commerciale de la ville en plein cœur de New York. A quelques minutes se trouve la place du temps et le centre historique de la ville, ainsi que d'autres lieux d'intérêt qui font de New York l'une des villes les plus attractives et cosmopolites de l'Amérique.",
                Category = "Boutique",
                Tags = new[] { "pool", "air conditioning", "concierge" },
                ParkingIncluded = false,
                LastRenovationDate = new DateTimeOffset(1970, 1, 18, 0, 0, 0, TimeSpan.Zero),
                Rating = 3,
                Roles = new[] { "admin" },
                Location = GeographyPoint.Create(40.760586, -73.975403)
            },
            new()
            {
                HotelId = Guid.NewGuid().ToString(),
                HotelName = "Twin Dome Motel",
                BaseRate = 235.00,
                Description = "The hotel is situated in a  nineteenth century plaza, which has been expanded and renovated to the highest architectural standards to create a modern, functional and first-class hotel in which art and unique historical elements coexist with the most modern comforts.",
                DescriptionFr = "L'hôtel est situé dans une place du XIXe siècle, qui a été agrandie et rénovée aux plus hautes normes architecturales pour créer un hôtel moderne, fonctionnel et de première classe dans lequel l'art et les éléments historiques uniques coexistent avec le confort le plus moderne.",
                Category = "Boutique",
                Tags = new[] { "pool", "free wifi", "concierge" },
                ParkingIncluded = false,
                LastRenovationDate = new DateTimeOffset(1979, 2, 18, 0, 0, 0, TimeSpan.Zero),
                Rating = 3,
                Roles = new[] { "admin", "member" },
                Location = GeographyPoint.Create(27.384417, -82.452843)
            },
            new()
            {
                HotelId = Guid.NewGuid().ToString(),
                HotelName = "Triple Landscape Hotel",
                BaseRate = 215.00,
                Description = "The Hotel stands out for its gastronomic excellence under the management of William Dough, who advises on and oversees all of the Hotel’s restaurant services.",
                DescriptionFr = "L'hôtel est situé dans une place du XIXe siècle, qui a été agrandie et rénovée aux plus hautes normes architecturales pour créer un hôtel moderne, fonctionnel et de première classe dans lequel l'art et les éléments historiques uniques coexistent avec le confort le plus moderne.",
                Category = "Resort and Spa",
                Tags = new[] { "air conditioning", "bar", "continental breakfast" },
                ParkingIncluded = true,
                LastRenovationDate = new DateTimeOffset(2015, 9, 20, 0, 0, 0, TimeSpan.Zero),
                Rating = 4,
                Roles = new[] { "nonmember", "admin", "member" },
                Location = GeographyPoint.Create(33.84643, -84.362465),

            },
            new()
            {
                HotelId = Guid.NewGuid().ToString(),
                HotelName = "Excalibur Hotel & Casino",
                BaseRate = 375.00,
                Description = "Family-friendly resort with spa, near Excalibur Casino.",
                DescriptionFr = "Complexe qui plaira aux familles avec spa, non loin de Casino Excalibur.",
                Category = "Resort and Spa",
                Tags = new[] { "air conditioning", "gym", "pool", "parking available", "fitness center", "breakfast available" },
                ParkingIncluded = true,
                LastRenovationDate = new DateTimeOffset(2015, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Rating = 4,
                Roles = new[] { "nonmember", "admin", "member" },
                Location = GeographyPoint.Create(36.09867, -115.17517),

            },
            new()
            {
                HotelId = Guid.NewGuid().ToString(),
                HotelName = "Luxor Hotel and Casino",
                BaseRate = 395.00,
                Description = "Resort with 11 restaurants, near Casino at Luxor Las Vegas",
                DescriptionFr = "Complexe avec 11 restaurants, non loin de Casino at Luxor Las Vegas.",
                Category = "Resort and Spa",
                Tags = new[] { "air conditioning", "gym", "pool", "parking available" },
                ParkingIncluded = true,
                LastRenovationDate = new DateTimeOffset(2018, 3, 1, 0, 0, 0, TimeSpan.Zero),
                Roles = new[] { "nonmember", "admin", "member" },
                Rating = 4,
                Location = GeographyPoint.Create(36.09572, -115.17620),

            }
        };

        return result;
    }
}