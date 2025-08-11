using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Response;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Application.CommandHandler
{
    public class AuthenticationCommands : IAuthenticationCommands
    {

        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationCommands(
            IPasswordService passwordService,
            ITokenService tokenService,
            IUnitOfWork unitOfWork)

        {
            _passwordService = passwordService;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }
        public async Task<AuthenticationResponse> SignInAsync(string email, string password, CancellationToken cancellationToken)
        {
            User? existingUser = await _unitOfWork.Users.GetByEmailAsync(email);
            if (existingUser is null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Invalid email or password"
                };
            }
            bool isValid = _passwordService.ValidatePassword(email, password, existingUser.PasswordHash);
            if (!isValid)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Invalid email or password"
                };
            }
            existingUser.UpdateLastLogin(DateTime.UtcNow);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Login Successfull",
                AccessToken = _tokenService.GenerateToken(existingUser!)
            };
        }

        public async Task<AuthenticationResponse> RegisterAsync(string name, string email, string password, List<Guid> rolesId, CancellationToken cancellationToken)
        {
            User? existingUser = await _unitOfWork.Users.GetByEmailAsync(email);
            if (existingUser is not null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "User already exists with this email."
                };
            }

            string passwordHash = _passwordService.HashPassword(email, password);

            var allRoles = await _unitOfWork.Roles.GetAllRolesAsync();
            List<Role> selectedRoles = allRoles.Where(r => rolesId.Contains(r.Id.Value)).ToList();

            if (!selectedRoles.Any())
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "No valid roles found."
                };
            }


            User user = User.Create(name, email, passwordHash, selectedRoles);
            await _unitOfWork.Users.AddAsync(user);

            Member newMember = new()
            {
                Id = new MemberId(user.Id.Value),
                Name = user.Name,
                Email = user.Email,
                MaxBooksAllowed = 5,
                BorrowedBooksCount = 0,
               
            };
            await _unitOfWork.Members.CreateMemberAsync(newMember);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            string accessToken = _tokenService.GenerateToken(user);
            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "User registered successfully.",
                AccessToken = accessToken
            };
        }


    }
}
