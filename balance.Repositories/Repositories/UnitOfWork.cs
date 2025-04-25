using balance.domain;
using balance.Repositories.Interfaces;

namespace balance.Repositories.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context;
        private IPropertyRepository _propertyRepository;
        private IPropertyTypeRepository _propertyTypeRepository;
        private IProjectRepository _projectRepository;
        private IFavoriteRepository _favoriteRepository;

        public UnitOfWork(Context context)
        {
            _context = context;
        }

        public IPropertyRepository Properties 
        { 
            get 
            {
                return _propertyRepository ??= new PropertyRepository(_context);
            } 
        }

        public IPropertyTypeRepository PropertyTypes 
        { 
            get 
            {
                return _propertyTypeRepository ??= new PropertyTypeRepository(_context);
            } 
        }

        public IProjectRepository Projects 
        { 
            get 
            {
                return _projectRepository ??= new ProjectRepository(_context);
            } 
        }

        public IFavoriteRepository Favorites 
        { 
            get 
            {
                return _favoriteRepository ??= new FavoriteRepository(_context);
            } 
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
