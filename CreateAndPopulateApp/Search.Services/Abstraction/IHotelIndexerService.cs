﻿namespace Search.Services;

public interface IHotelIndexerService
{
    Task CreateIndexerAsync(string indexerName, string dataSourceName, string targetIndexName);
}