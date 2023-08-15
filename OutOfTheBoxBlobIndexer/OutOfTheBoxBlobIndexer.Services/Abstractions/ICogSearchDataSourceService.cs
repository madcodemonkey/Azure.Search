﻿using OutOfTheBoxBlobIndexer.Models;

namespace OutOfTheBoxBlobIndexer.Services;

public interface ICogSearchDataSourceService
{
    /// <summary>Creates a Azure SQL data source that will be used by an indexer.</summary>
    /// <param name="name">The name of the data source</param>
    /// <param name="tableOrViewName">The table or view that the Azure SQL data source is pointed at.</param>
    /// <param name="connectionString">A connection string to attach to the database.</param>
    /// <param name="highWaterMarkColumnName">The high watermark field used to detect changes</param>
    /// <param name="softDeleteColumnName">The column that indicates that the record should be removed from the Azure Search Index</param>
    /// <param name="softDeleteColumnValue">The value in the column that indicates that the record should be deleted.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    Task<bool> CreateForAzureSqlAsync(string name, string tableOrViewName, string connectionString,
        string highWaterMarkColumnName, string softDeleteColumnName, string softDeleteColumnValue, CancellationToken cancellationToken = default);

    /// <summary>Creates a blob storage data source that will be used an out-of-the-box indexer.</summary>
    /// <param name="name">The name of the data source</param>
    /// <param name="containerName">The name of the container within the Azure storage resource.</param>
    /// <param name="connectionString">A connection string to attach to the Azure storage resource.</param>
    /// <param name="softDeleteColumnName">The column that indicates that the record should be removed from the Azure Search Index</param>
    /// <param name="softDeleteColumnValue">The value in the column that indicates that the record should be deleted.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    Task<bool> CreateForBlobAsync(string name, string containerName, string connectionString,
        string softDeleteColumnName, string softDeleteColumnValue, CancellationToken cancellationToken = default);

    /// <summary>Creates a Cosmos DB Apache Gremlin data source that will be used by an indexer.</summary>
    /// <param name="name">The name of the data source</param>
    /// <param name="containerName">The container that houses the documents</param>
    /// <param name="connectionString">A connection string to attach to Cosmos DB Apache Gremlin.</param>
    /// <param name="queryType">The type of query to perform (vertices or edges)</param>
    /// <param name="softDeleteColumnName">The column that indicates that the record should be removed from the Azure Search Index</param>
    /// <param name="softDeleteColumnValue">The value in the column that indicates that the record should be deleted.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <remarks>
    /// See docs: https://learn.microsoft.com/en-us/azure/search/search-howto-index-cosmosdb-gremlin#define-the-data-source
    /// </remarks>
    Task<bool> CreateForGremlinAsync(string name, string containerName, string connectionString,
        GremlinQueryTypes queryType, string? softDeleteColumnName, string? softDeleteColumnValue, CancellationToken cancellationToken = default);

    /// <summary>Delete a data sources</summary>
    /// <param name="dataSourceConnectionName">The name of the data source</param>
    /// <param name="checkIfExistsFirst">Indicates if you want the code to check to make sure the indexer exists before attempting to delete it.  If you try
    /// to delete an indexer that doesn't exist, it will generate an exception.</param>   /// <param name="cancellationToken">A cancellation token</param>
    Task<bool> DeleteAsync(string dataSourceConnectionName, bool checkIfExistsFirst, CancellationToken cancellationToken = default);

    /// <summary>Gets a list of data sources</summary>
    /// <param name="dataSourceConnectionName">The name of the data source</param>
    /// <param name="cancellationToken">A cancellation token</param>
    Task<bool> ExistsAsync(string dataSourceConnectionName, CancellationToken cancellationToken = default);

    /// <summary>Gets a list of data sources</summary>
    /// <param name="cancellationToken">A cancellation token</param>
    Task<List<string>> GetListAsync(CancellationToken cancellationToken = default);
}