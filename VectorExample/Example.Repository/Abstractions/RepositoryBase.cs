using Microsoft.EntityFrameworkCore;

namespace Example.Repository;

/// <summary>
/// Extends <see cref="RepositoryBase{TEntity}"/> to support actions with using a primary key of type <typeparamref name="TPrimaryKey"/>
/// </summary>
public abstract class RepositoryBase<TEntity, TPrimaryKey> : RepositoryBase<TEntity> where TEntity : class
{
    protected RepositoryBase(AppDbContext context) : base(context)
    { }

    /// <summary>Retrieves one entity from the database.</summary>
    /// <param name="id">The primary key</param>
    /// <param name="cancellationToken">The cancellation token</param>
    public virtual async Task<TEntity?> GetAsync(TPrimaryKey id, CancellationToken cancellationToken = default) => await DbSet.FindAsync(id, cancellationToken);

    /// <summary>Deletes one entity from the database.</summary>
    /// <param name="id">The primary key</param>
    /// <param name="saveChanges">Indicates if we should save immediately or not</param>
    /// <param name="cancellationToken">The cancellation token</param>
    public virtual async Task DeleteAsync(TPrimaryKey id, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var objectToDelete = await DbSet.FindAsync(id, cancellationToken);
        if (objectToDelete != null)
        {
            await DeleteAsync(objectToDelete, saveChanges, cancellationToken);
        }
    }
}

/// <summary>
/// Provides base repository functionality for <typeparamref name="TEntity"/>
/// </summary>
public abstract class RepositoryBase<TEntity> where TEntity : class
{
    protected readonly AppDbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    /// <summary>Constructor</summary>
    protected RepositoryBase(AppDbContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        DbSet = context.Set<TEntity>();
    }

    /// <summary>Inserts one entity into the database.</summary>
    /// <param name="entity">The entity to add</param>
    /// <param name="saveChanges">Indicates if we should save immediately or not</param>
    /// <param name="cancellationToken">The cancellation token</param>
    public virtual async Task<TEntity> AddAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        if (saveChanges)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }

        return entity;
    }

    /// <summary>Retrieves all entities from the database.</summary>
    /// <param name="cancellationToken">The cancellation token</param>
    public virtual async Task<List<TEntity>> GetAsync(CancellationToken cancellationToken = default) => await DbSet.ToListAsync(cancellationToken);

    /// <summary>Deletes one entity from the database.</summary>
    /// <param name="entity">Entity to delete</param>
    /// <param name="saveChanges">Indicates if we should save immediately or not</param>
    /// <param name="cancellationToken">The cancellation token</param>
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
    /// <param name="cancellationToken">The cancellation token</param>
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

    /// <summary>Saves Changes.</summary>
    /// <param name="cancellationToken">The cancellation token</param>
    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await Context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>Updates one entity in the database.</summary>
    /// <param name="entity">Entity to update</param>
    /// <param name="saveChanges">Indicates if we should save immediately or not</param>
    /// <param name="cancellationToken">The cancellation token</param>
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