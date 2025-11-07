namespace InventoryTrackSystem.Model.Dtos.Auth
{
    public class LoginResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string? Email { get; set; }
    }
}
