using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Generic repository implementation providing basic CRUD operations for entities.
    /// Implements the IRepository interface.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ResumeDbContext _context;
        private readonly DbSet<T> _dbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="context">Database context for accessing data.</param>
        public Repository(ResumeDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        /// Retrieves an entity by its ID.
        /// </summary>
        /// <param name="id">Entity ID.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Retrieves all entities of type T.
        /// </summary>
        /// <returns>A list of entities.</returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Adds a new entity to the database.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        /// <summary>
        /// Deletes an entity from the database.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Saves all pending changes to the database.
        /// </summary>
        /// <returns>True if changes were saved successfully; otherwise, false.</returns>
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
