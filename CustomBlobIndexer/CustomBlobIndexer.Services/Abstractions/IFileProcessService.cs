﻿namespace CustomBlobIndexer.Services;

public interface IFileProcessService
{
    Task ProcessFileAsync(string name, Uri uri, CancellationToken cancellationToken = default);
}