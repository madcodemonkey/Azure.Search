using Search.Model;

namespace Search.Repositories;

public class IndexConfigurationRepository : RepositoryPrimaryKeyBase<IndexConfiguration, AcmeContext, string>, IIndexConfigurationRepository
{
    /// <summary>Constructor</summary>
    public IndexConfigurationRepository(AcmeContext context) : base(context)
    {

    }
}