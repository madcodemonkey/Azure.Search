namespace Search.CogServices;

public interface IAcmeIndexInfoService
{
    string GetSecurityTrimmingFieldName(string indexName);

    bool UseSecurityTrimming(string indexName);
}