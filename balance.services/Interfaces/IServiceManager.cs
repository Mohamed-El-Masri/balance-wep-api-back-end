namespace balance.services.Interfaces
{
    public interface IServiceManager
    {
        IPropertyService PropertyService { get; }
        IPropertyTypeService PropertyTypeService { get; }
        IProjectService ProjectService { get; }
        IFavoriteService FavoriteService { get; }
        IAuthenticationService AuthenticationService { get; }
        IUserProfileService UserProfileService { get; }
        ITokenService TokenService { get; }
    }
}
