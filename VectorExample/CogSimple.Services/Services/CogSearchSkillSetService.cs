using Azure;

namespace CogSimple.Services;

public class CogSearchSkillSetService : ICogSearchSkillSetService
{
    protected ICogClientWrapperService ClientService { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    public CogSearchSkillSetService(ICogClientWrapperService clientService)
    {
        ClientService = clientService;
    }


    /// <summary>Delete a data sources</summary>
    /// <param name="skillSetName">The name of the skill set</param>
    /// <param name="checkIfExistsFirst">Indicates if you want the code to check to make sure the skill set exists before attempting to delete it.  If you try
    /// to delete an skill set that doesn't exist, it will generate an exception.</param> 
    /// <param name="cancellationToken">A cancellation token</param>
    public async Task<bool> DeleteAsync(string skillSetName, bool checkIfExistsFirst, CancellationToken cancellationToken = default)
    {
        if (checkIfExistsFirst && await ExistsAsync(skillSetName, cancellationToken) == false)
        {
            return false;
        }

        var indexerClient = ClientService.GetIndexerClient();


        await indexerClient.DeleteSkillsetAsync(skillSetName, cancellationToken: cancellationToken);
        return true;
    }

    /// <summary>Checks to see if a skill set exists</summary>
    /// <param name="skillSetName">The name of the skill set</param>
    /// <param name="cancellationToken">A cancellation token</param>
    public async Task<bool> ExistsAsync(string skillSetName, CancellationToken cancellationToken = default)
    {
        var indexerClient = ClientService.GetIndexerClient();

        Response<IReadOnlyList<string>>? response = await indexerClient.GetSkillsetNamesAsync(cancellationToken);

        foreach (var item in response.Value)
        {
            if (string.IsNullOrWhiteSpace(item)) continue;
            if (skillSetName == item) return true;
        }

        return false;
    }

    /// <summary>Gets a list of skill sets</summary>
    /// <param name="cancellationToken">A cancellation token</param>
    public async Task<List<string>> GetListAsync(CancellationToken cancellationToken = default)
    {
        var indexerClient = ClientService.GetIndexerClient();

        Response<IReadOnlyList<string>> response = await indexerClient.GetSkillsetNamesAsync(cancellationToken);

        List<string> result = response.Value.ToList();

        return result;
    }
}