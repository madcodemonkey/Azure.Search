﻿using Search.CogServices;

namespace IndexHelper.Services;

/// <summary>
/// Used to create the index (see method below) and search (see the base class) the index.
/// </summary>
public interface IPersonIndexService : IAcmeCogIndexService
{
    /// <summary>
    /// Create index or update the index.
    /// </summary>
    void CreateOrUpdateIndex();
}