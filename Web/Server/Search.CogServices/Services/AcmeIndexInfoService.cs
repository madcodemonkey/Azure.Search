namespace Search.CogServices;

public class AcmeIndexInfoService : IAcmeIndexInfoService
{

    public string GetSecurityTrimmingFieldName(string indexName)
    {
        // TODO: Get this from appsettings for now.
        return "role";
    }

    public bool UseSecurityTrimming(string indexName)
    {
        return true;
    }
}