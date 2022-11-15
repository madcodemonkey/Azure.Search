using Search.CogServices;
using Search.Model;

namespace Search.Services;

public class HotelFieldService : AcmeFieldServiceBase, IHotelFieldService
{

    /// <summary>Register all the fields that are marked as IsFilterable or IsFacetable
    /// here.  We do NOT want the client side building filters.  It can only pass in
    /// query text.  This avoids injection attacks where the client can see anything in the
    /// index.  If they could build them, they could avoid security trimming.</summary>
    protected override List<IAcmeSearchField> RegisterFields()
    {
        var fieldList = new List<IAcmeSearchField>();
    

        // SimpleField attribute can be any data type, is always non-searchable (it's ignored for full text search queries), and is retrievable (it's not hidden). 
        // IsSearchable - class properties must be decorated with the SearchableField attribute and must be string properties, and is always searchable and retrievable. 
        // IsFacetable	Gets or sets a value indicating whether the field can be retrieved in facet queries. 
        // IsFilterable	Gets or sets a value indicating whether the field can be referenced in $filter queries. 
        // IsSortable	Gets or sets a value indicating whether to enable the field can be referenced in $orderby expressions. 

        // Note: Yes, I could use reflection to get this info; however, I'm not because
        //       1. I'm adding additional info (security trimming) that is not on the attribute
        //       2. I'm using an email to set the field id and that enum is going to be in the client side code as well.
        
        fieldList.Add(new AcmeSearchFieldForNumberField((int) HotelDocumentFieldEnum.BaseRate, "baseRate", "Base Rate", true, true, true, false, false));
        fieldList.Add(new AcmeSearchFieldForStringField((int) HotelDocumentFieldEnum.Category, "category", "Category", true, true, true, true, false));
        fieldList.Add(new AcmeSearchFieldForStringField((int) HotelDocumentFieldEnum.Description, "description", "Description", false, false, false, true, false));
        fieldList.Add(new AcmeSearchFieldForStringField((int) HotelDocumentFieldEnum.HotelId, "hotelId", "Id", true, false, false, false, false));
        fieldList.Add(new AcmeSearchFieldForStringField((int) HotelDocumentFieldEnum.HotelName, "hotelName", "Hotel Name", true, true, false, true, false));
        fieldList.Add(new AcmeSearchFieldForBooleanField((int) HotelDocumentFieldEnum.LastRenovationDate, "lastRenovationDate", "Last Renovation Date", true, true, false, false, false));
        fieldList.Add(new AcmeSearchFieldForBooleanField((int) HotelDocumentFieldEnum.ParkingIncluded, "parkingIncluded", "Parking Included", true, false, true, false, false));
        fieldList.Add(new AcmeSearchFieldForBooleanField((int) HotelDocumentFieldEnum.SmokingAllowed, "smokingAllowed", "Smoking Allowed", true, false, true, false, false));
        fieldList.Add(new AcmeSearchFieldForStringField((int) HotelDocumentFieldEnum.Rating, "rating", "Rating", true, true, true, false, false));
        fieldList.Add(new AcmeSearchFieldForStringCollection((int) HotelDocumentFieldEnum.Roles, "roles", "Roles", true, false, false, false, true)); // Security Trimming field
        fieldList.Add(new AcmeSearchFieldForStringField((int) HotelDocumentFieldEnum.Tags, "tags", "Tags", true, false, true, false, false));

        return fieldList;
    }
}