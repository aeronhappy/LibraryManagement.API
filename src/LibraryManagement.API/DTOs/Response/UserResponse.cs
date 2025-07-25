namespace LibraryManagement.API.DTOs.Response
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public List<RoleResponse> Roles { get; set; } = [];
    }
}
