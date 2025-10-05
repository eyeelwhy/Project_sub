namespace dockers_api.Classes
{
    public class UserRegDTO
    {
        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
