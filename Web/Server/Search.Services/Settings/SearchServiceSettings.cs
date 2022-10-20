﻿namespace Search.Services;

public class SearchServiceSettings
{
    public string SearchEndPoint { get; set; }
    public string SearchApiKey { get; set; }

    public string DatabaseConnectionString { get; set; }   // TODO: Get rid of this one and use the database options
    public string HotelDataSourceName { get; set; }

    public string HotelSuggestorName { get; set; }
    public string HotelIndexName { get; set; }
    public string HotelSynonymMapName { get; set; }
    public string HotelTableName { get; set; }
    public string HotelIndexerName { get; set; }
}

