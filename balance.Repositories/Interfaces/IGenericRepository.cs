using balance.domain.Common;
using System.Linq.Expressions;

namespace balance.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        // Get all entities
        IQueryable<T> GetAll();
        
        // Get entities with include properties
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null,
                            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                            string includeProperties = "");
                            
        // Get by ID
        Task<T> GetByIdAsync(int id);
        
        // Add new entity
        Task AddAsync(T entity);
        
        // Add range of entities
        Task AddRangeAsync(IEnumerable<T> entities);
        
        // Update entity
        void Update(T entity);
        
        // Delete entity
        Task DeleteAsync(int id);
        
        // Delete entity
        void Delete(T entity);
        
        // Delete range of entities
        void DeleteRange(IEnumerable<T> entities);
        
        // Find with expression
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
    }
}
