using Identity.Application.Commands;
using Identity.Application.Response;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Identity.Domain.Repositories;
using LibraryManagement.Contracts.Services;

namespace Identity.Application.CommandHandler
{
    public class AuthenticationCommands : IAuthenticationCommands
    {

        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDomainEventPublisher _domainEventPublisher;

        public AuthenticationCommands(
            IPasswordService passwordService,
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            IDomainEventPublisher domainEventPublisher)


        {
            _passwordService = passwordService;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _domainEventPublisher = domainEventPublisher;
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
                AccessToken = _tokenService.GenerateToken(existingUser!),
                Roles = existingUser.Roles.Select(r => r.Name).ToList()
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
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _domainEventPublisher.PublishAsync(user.DomainEvents, default);


            string accessToken = _tokenService.GenerateToken(user);
            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "User registered successfully.",
                AccessToken = accessToken,
                Roles = user.Roles.Select(r => r.Name).ToList()

            };
        }


    }
}
