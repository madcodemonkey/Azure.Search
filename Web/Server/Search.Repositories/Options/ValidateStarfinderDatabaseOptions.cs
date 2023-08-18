using Microsoft.Extensions.Options;

namespace Search.Repositories;

public class ValidateAcmeDatabaseOptions : IValidateOptions<AcmeDatabaseOptions>
{
    public ValidateOptionsResult Validate(string name, AcmeDatabaseOptions options)
    {
        string? result = null;

        if (string.IsNullOrWhiteSpace(options.ConnectionString))
        {
            result += $"{nameof(options.ConnectionString)} must be provided.\n";
        }

        return result != null
            ? ValidateOptionsResult.Fail(result)
            : ValidateOptionsResult.Success;
    }
}