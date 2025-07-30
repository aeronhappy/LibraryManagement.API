using AutoMapper;
using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Response;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Application.CommandHandler
{
    public class AuthenticationCommandService : IAuthenticationCommandService
    {

        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationCommandService(
            IPasswordService passwordService,
            ITokenService tokenService,
            IMapper mapper,
            IUnitOfWork unitOfWork
            )


        {
         
            _passwordService = passwordService;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<AuthenticationResponse> SignInAsync(string email, string password)
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
            await _unitOfWork.SaveChangesAsync(default);
            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Login Successfull",
                AccessToken = _tokenService.GenerateToken(existingUser!)
            };
        }

        public async Task<AuthenticationResponse> RegisterAsync(string name, string email, string password, List<Guid> rolesId)
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
            await _unitOfWork.SaveChangesAsync(default);

            string accessToken = _tokenService.GenerateToken(user);
            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "User registered successfully.",
                AccessToken = accessToken
            };
        }


        public async Task<UserResponse?> GetUserByIdAsync(Guid id)
        {
            UserResponse userResponse = await _unitOfWork.Users.GetByIdAsync(new UserId(id))
                  .ContinueWith(t => _mapper.Map<UserResponse>(t.Result));
            if (userResponse is null)
                return null;
            return userResponse;
        }

        public async Task<List<UserResponse>> GetAllUsersAsync()
        {
            return await _unitOfWork.Users.GetUsersAsync()
                 .ContinueWith(t => t.Result.Select(u => _mapper.Map<UserResponse>(u)).ToList());
        }
    }
}
