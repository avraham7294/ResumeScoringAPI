using System.Linq.Expressions;

namespace Application.Interfaces
{
    /// <summary>
    /// Generic Repository interface for basic CRUD operations.
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">Unique identifier of the entity.</param>
        /// <returns>Entity if found; otherwise, null.</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves all entities of type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>Collection of entities.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Adds a new entity to the data source.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        Task AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity in the data source.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        void Update(T entity);

        /// <summary>
        /// Deletes an entity from the data source.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        void Delete(T entity);

        /// <summary>
        /// Persists all changes made in the current context to the database.
        /// </summary>
        /// <returns>True if changes were saved successfully; otherwise, false.</returns>
        Task<bool> SaveChangesAsync();
    }
}
