using AutoMapper;
using balance.domain;
using balance.Repositories.Interfaces;
using balance.services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace balance.services.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IPropertyService> _propertyService;
        private readonly Lazy<IPropertyTypeService> _propertyTypeService;
        private readonly Lazy<IProjectService> _projectService;
        private readonly Lazy<IFavoriteService> _favoriteService;
        private readonly Lazy<IAuthenticationService> _authenticationService;
        private readonly Lazy<IUserProfileService> _userProfileService;
        private readonly Lazy<ITokenService> _tokenService;
        
        public ServiceManager(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            UserManager<Applicationuser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _propertyService = new Lazy<IPropertyService>(() => 
                new PropertyService(unitOfWork, mapper));
                
            _propertyTypeService = new Lazy<IPropertyTypeService>(() => 
                new PropertyTypeService(unitOfWork, mapper));
                
            _projectService = new Lazy<IProjectService>(() => 
                new ProjectService(unitOfWork, mapper));
                
            _favoriteService = new Lazy<IFavoriteService>(() => 
                new FavoriteService(unitOfWork, mapper));

            _tokenService = new Lazy<ITokenService>(() =>
                new TokenService(configuration));
                
            _authenticationService = new Lazy<IAuthenticationService>(() => 
                new AuthenticationService(userManager, roleManager, _tokenService.Value));
                
            _userProfileService = new Lazy<IUserProfileService>(() => 
                new UserProfileService(userManager, mapper));
        }
        
        public IPropertyService PropertyService => _propertyService.Value;
        public IPropertyTypeService PropertyTypeService => _propertyTypeService.Value;
        public IProjectService ProjectService => _projectService.Value;
        public IFavoriteService FavoriteService => _favoriteService.Value;
        public IAuthenticationService AuthenticationService => _authenticationService.Value;
        public IUserProfileService UserProfileService => _userProfileService.Value;
        public ITokenService TokenService => _tokenService.Value;
    }
}
