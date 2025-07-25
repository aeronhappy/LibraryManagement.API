namespace LibraryManagement.API.Services.Interface
{
    public interface IPasswordService
    {
        string HashPassword(string email, string password);
        bool ValidatePassword(string email, string password, string passwordHash);
    }
}
