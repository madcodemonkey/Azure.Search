using Microsoft.EntityFrameworkCore;

namespace CustomSqlServerIndexer.Repositories;

/// <summary>
/// Extends <see cref="RepositoryBase{TEntity,TDatabaseContext}"/> to support actions with using a primary key of type <typeparamref name="TPrimaryKey"/>
/// </summary>
public abstract class RepositoryPrimaryKeyBase<TEntity, TDatabaseContext, TPrimaryKey> :
    RepositoryBase<TEntity, TDatabaseContext>, IRepositoryPrimaryKeyBase<TEntity, TPrimaryKey>
    where TEntity : class
    where TDatabaseContext : DbContext
{
    /// <summary>Constructor</summary>
    protected RepositoryPrimaryKeyBase(TDatabaseContext context) : base(context)
    { }

    /// <summary>Deletes one entity from the database.</summary>
    /// <param name="id">The primary key</param>
    /// <param name="saveChanges">Indicates if we should save immediately or not</param>
    public virtual async Task DeleteAsync(TPrimaryKey id, bool saveChanges = true)
    {
        var objectToDelete = await DbSet.FindAsync(id);
        if (objectToDelete != null)
        {
            await DeleteAsync(objectToDelete, saveChanges);
        }
    }

    /// <summary>Retrieves one entity from the database.</summary>
    /// <param name="id">The primary key</param>
    public virtual async Task<TEntity?> GetAsync(TPrimaryKey id) => await DbSet.FindAsync(id);
}