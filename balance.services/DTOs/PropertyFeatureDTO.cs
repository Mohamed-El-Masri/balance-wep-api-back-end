namespace balance.services.DTOs
{
    public class PropertyFeatureDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public int PropertyId { get; set; }
    }
}
