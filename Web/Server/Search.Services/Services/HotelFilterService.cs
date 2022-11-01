using Search.CogServices;

namespace Search.Services;

public class HotelFilterService : AcmeFilterServiceBase, IHotelFilterService
{

    /// <summary>Register all the fields that are marked as IsFilterable or IsFacetable
    /// here.  We do NOT want the client side building filters.  It can only pass in
    /// query text.  This avoids injection attacks where the client can see anything in the
    /// index.  If they could build them, they could avoid security trimming.</summary>
    protected override List<IAcmeSearchFilter> RegisterFilters()
    {
        var filterList = new List<IAcmeSearchFilter>();
        int id = 1;

        filterList.Add(new AcmeSearchFilterForStringField(id++, "hotelId", "Id", false, false));
        filterList.Add(new AcmeSearchFilterForNumberField(id++, "baseRate", "Base Rate", true, false));
        filterList.Add(new AcmeSearchFilterForStringField(id++, "description", "Description", false, false));
        filterList.Add(new AcmeSearchFilterForStringField(id++, "hotelName", "Hotel Name", false, false));
        filterList.Add(new AcmeSearchFilterForStringField(id++, "category", "Category", true, false));
        filterList.Add(new AcmeSearchFilterForStringField(id++, "tags", "Tags", true, false));
        filterList.Add(new AcmeSearchFilterForBooleanField(id++, "parkingIncluded", "Parking Included", true, false));
        filterList.Add(new AcmeSearchFilterForBooleanField(id++, "smokingAllowed", "Smoking Allowed", true, false));
        filterList.Add(new AcmeSearchFilterForStringField(id++, "rating", "Rating", true, false));

        filterList.Add(new AcmeSearchFilterForStringCollection(id++, "roles", "Roles", false, true)); // Security Trimming field

        return filterList;
    }
}