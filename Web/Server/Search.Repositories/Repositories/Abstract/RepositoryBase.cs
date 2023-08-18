using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Search.Repositories;

/// <summary>
/// Provides base repository functionality for <typeparamref name="TEntity"/>
/// </summary>
public abstract class RepositoryBase<TEntity, TDatabaseContext> : IRepositoryBase<TEntity>
    where TEntity : class
    where TDatabaseContext : DbContext
{
    protected readonly TDatabaseContext Context;
    protected readonly DbSet<TEntity> DbSet;

    /// <summary>Constructor</summary>
    protected RepositoryBase(TDatabaseContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        DbSet = context.Set<TEntity>();
    }

    /// <summary>Inserts one entity into the database.</summary>
    /// <param name="entity">The entity to add</param>
    /// <param name="saveChanges">Indicates if we should save immediately or not</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public virtual async Task<TEntity> AddAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        if (saveChanges)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }
        return entity;
    }

    /// <summary>Deletes one entity from the database.</summary>
    /// <param name="entity">Entity to delete</param>
    /// <param name="saveChanges">Indicates if we should save immediately or not</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public virtual async Task DeleteAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        DbSet.Remove(entity);
        if (saveChanges)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>Deletes a list of entity from the database.</summary>
    /// <param name="items">Items to delete</param>
    /// <param name="saveChanges">Indicates if we should save immediately or not</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public virtual async Task DeleteAsync(List<TEntity> items, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        foreach (var entity in items)
        {
            DbSet.Remove(entity);
        }

        if (saveChanges)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>Retrieves all entities from the database.</summary>
    public virtual async Task<List<TEntity>> GetAsync(CancellationToken cancellationToken = default) => await DbSet.ToListAsync(cancellationToken);

    /// <summary>Retrieves all entities from the database.</summary>
    public virtual async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) => await DbSet.Where(predicate).ToListAsync(cancellationToken);

    /// <summary>Returns the first item or null (nothing included).</summary>
    public virtual async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) => await DbSet.FirstOrDefaultAsync(predicate, cancellationToken);

    /// <summary>Returns the first item or null  (includes specified relationships).</summary>
    /// <param name="predicate">A where clause predicate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="includes">Relationships to include</param>
    public virtual async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object?>>[] includes)
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate), "You must specify a predicate!");

        var query = DbSet.Where(predicate);

        if (includes.Any())
        {
            foreach (var include in includes)
                query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>Retrieves all entities with the specified includes.</summary>
    /// <param name="predicate">A where clause predicate</param>
    /// <param name="includes">Foreign key relationships that you would like to include</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public virtual async Task<List<TEntity>> GetWithIncludesAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object?>>[] includes)
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate), "You must specify a predicate!");

        var query = DbSet.Where(predicate);

        if (includes.Any())
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.ToListAsync(cancellationToken);
    }

    /// <summary>Updates one entity in the database.</summary>
    /// <param name="entity">Entity to update</param>
    /// <param name="saveChanges">Indicates if we should save immediately or not</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public virtual async Task UpdateAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        DbSet.Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;

        if (saveChanges)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }
    }
}