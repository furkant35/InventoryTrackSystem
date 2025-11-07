namespace InventoryTrackSystem.Model.Dtos.Auth
{
    public class CurrentUserDto
    {
        public bool IsAuthenticated { get; set; }
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; }
    }
}
