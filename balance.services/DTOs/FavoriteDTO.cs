namespace balance.services.DTOs
{
    public class FavoriteDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int PropertyId { get; set; }
        public string PropertyTitle { get; set; }
        public decimal PropertyPrice { get; set; }
        public string PropertyAddress { get; set; }
        public string PropertyImageUrl { get; set; }
        public string Notes { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
