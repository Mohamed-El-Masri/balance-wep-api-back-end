using balance.Repositories.Interfaces;

namespace balance.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPropertyRepository Properties { get; }
        IPropertyTypeRepository PropertyTypes { get; }
        IProjectRepository Projects { get; }
        IFavoriteRepository Favorites { get; }
        
        Task<int> CompleteAsync();
    }
}
