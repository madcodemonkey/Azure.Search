namespace CogSimple.Services;

public interface ICogSearchSkillSetService
{
    /// <summary>Delete a data sources</summary>
    /// <param name="skillSetName">The name of the skill set</param>
    /// <param name="checkIfExistsFirst">Indicates if you want the code to check to make sure the skill set exists before attempting to delete it.  If you try
    /// to delete an skill set that doesn't exist, it will generate an exception.</param> 
    /// <param name="cancellationToken">A cancellation token</param>
    Task<bool> DeleteAsync(string skillSetName, bool checkIfExistsFirst, CancellationToken cancellationToken = default);

    /// <summary>Checks to see if a skill set exists</summary>
    /// <param name="skillSetName">The name of the skill set</param>
    /// <param name="cancellationToken">A cancellation token</param>
    Task<bool> ExistsAsync(string skillSetName, CancellationToken cancellationToken = default);

    /// <summary>Gets a list of skill sets</summary>
    /// <param name="cancellationToken">A cancellation token</param>
    Task<List<string>> GetListAsync(CancellationToken cancellationToken = default);
}