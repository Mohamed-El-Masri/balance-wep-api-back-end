namespace balance.services.DTOs
{
    public class GeoLocationDTO
    {
        public int Id { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int PropertyId { get; set; }
        public int ProjectId { get; set; }
    }
}
