using System.Linq.Expressions;

namespace Search.Repositories;

public interface IRepositoryBase<TEntity> where TEntity : class
{
    /// <summary>Inserts one entity into the database.</summary>
    /// <param name="entity">The entity to add</param>
    /// <param name="saveChanges">Indicates if we should save immediately or not</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<TEntity> AddAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default);

    /// <summary>Deletes one entity from the database.</summary>
    /// <param name="entity">Entity to delete</param>
    /// <param name="saveChanges">Indicates if we should save immediately or not</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DeleteAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default);

    /// <summary>Deletes a list of entity from the database.</summary>
    /// <param name="items">Items to delete</param>
    /// <param name="saveChanges">Indicates if we should save immediately or not</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DeleteAsync(List<TEntity> items, bool saveChanges = true, CancellationToken cancellationToken = default);

    /// <summary>Retrieves all entities from the database.</summary>
    Task<List<TEntity>> GetAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves all entities from the database.</summary>
    Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>Returns the first item or null (nothing included).</summary>
    Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>Returns the first item or null  (includes specified relationships).</summary>
    /// <param name="predicate">A where clause predicate</param>
    /// <param name="includes">Relationships to include</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object?>>[] includes);

    /// <summary>Retrieves all entities with the specified includes.</summary>
    /// <param name="predicate">A where clause predicate</param>
    /// <param name="includes">Foreign key relationships that you would like to include</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<List<TEntity>> GetWithIncludesAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default, params Expression<Func<TEntity, object?>>[] includes);

    /// <summary>Updates one entity in the database.</summary>
    /// <param name="entity">Entity to update</param>
    /// <param name="saveChanges">Indicates if we should save immediately or not</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task UpdateAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default);
}