using AutoMapper;
using balance.domain;
using balance.Repositories.Interfaces;
using balance.services.DTOs;
using balance.services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace balance.services.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FavoriteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FavoriteDTO>> GetUserFavoritesAsync(string userId)
        {
            var favorites = await _unitOfWork.Favorites.GetWithPropertyDetailsAsync(userId);
            return _mapper.Map<IEnumerable<FavoriteDTO>>(favorites);
        }

        public async Task<bool> IsFavoritedByUserAsync(int propertyId, string userId)
        {
            return await _unitOfWork.Favorites.IsFavoritedByUserAsync(propertyId, userId);
        }

        public async Task<FavoriteDTO> AddToFavoritesAsync(int propertyId, string userId, string notes = null)
        {
            // Check if already favorited
            if (await IsFavoritedByUserAsync(propertyId, userId))
                return null;

            var favorite = new Favorite
            {
                PropertyId = propertyId,
                UserId = userId,
                Notes = notes,
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.Favorites.AddAsync(favorite);
            await _unitOfWork.CompleteAsync();

            // Fetch the full favorite with property details to return proper DTO
            var addedFavorite = await _unitOfWork.Favorites.GetAll(
                f => f.Id == favorite.Id,
                null,
                "Property,Property.Images,User")
                .FirstOrDefaultAsync();

            return _mapper.Map<FavoriteDTO>(addedFavorite);
        }

        public async Task<bool> RemoveFromFavoritesAsync(int propertyId, string userId)
        {
            var favorite = await _unitOfWork.Favorites.GetAll(
                f => f.PropertyId == propertyId && f.UserId == userId)
                .FirstOrDefaultAsync();

            if (favorite == null)
                return false;

            _unitOfWork.Favorites.Delete(favorite);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }

        public async Task<bool> UpdateFavoriteNotesAsync(int favoriteId, string notes)
        {
            var favorite = await _unitOfWork.Favorites.GetByIdAsync(favoriteId);
            if (favorite == null)
                return false;

            favorite.Notes = notes;
            favorite.ModifiedAt = DateTime.Now;

            _unitOfWork.Favorites.Update(favorite);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }
    }
}
