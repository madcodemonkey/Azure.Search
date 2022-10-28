﻿using Search.CogServices;
using Search.Model;

namespace Search.Services;

public interface IHotelSearchService
{
    /// <summary>Search</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="rolesTheUserIsAssigned">Case sensitive list of roles that for search trimming.</param>
    /// <returns>Results from index search</returns>
    Task<AcmeSearchQueryResult<HotelDocument>> SearchAsync(AcmeSearchQuery request, string[] rolesTheUserIsAssigned);
}