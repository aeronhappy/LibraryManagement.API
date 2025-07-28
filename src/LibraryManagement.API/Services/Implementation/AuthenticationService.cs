using AutoMapper;
using LibraryManagement.API.Datas;
using LibraryManagement.API.Datas.Models;
using LibraryManagement.API.DTOs.Response;
using LibraryManagement.API.Repositories.Interface;
using LibraryManagement.API.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryManagement.API.Services.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly ApplicationDbContext _context;

        public AuthenticationService(
            IUserRepository userRepository,
            IPasswordService passwordService, 
            ITokenService tokenService,
            IMapper mapper,
            IRoleRepository roleService,
            IMemberRepository memberRepository,
            ApplicationDbContext context
            )
            
          
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _mapper = mapper;
            _roleRepository = roleService;
            _memberRepository = memberRepository;
            _context = context;
        }
        public async Task<AuthenticationResponse> SignInAsync(string email, string password)
        {
            User? existingUser = await _userRepository.GetByEmailAsync(email);
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

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Login Successfull",
                AccessToken = _tokenService.GenerateToken(existingUser)
            };
        }

        public async Task<AuthenticationResponse> RegisterAsync(string name, string email, string password, List<Guid> rolesId)
        {
            User? existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser is not null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "User already exists with this email."
                };
            }

            string passwordHash = _passwordService.HashPassword(email, password);

            var allRoles = await _roleRepository.GetAllRolesAsync();
            List<Role> selectedRoles = allRoles.Where(r => rolesId.Contains(r.Id)).ToList();

            if (!selectedRoles.Any())
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "No valid roles found."
                };
            }
              

            User user = User.Create(name, email, passwordHash, selectedRoles);
            await _userRepository.AddAsync(user);

            Member newMember = new()
            {
                Id = Guid.NewGuid(),
                Name = user.Name,
                Email = user.Email,
                MaxBooksAllowed = 5,
                BorrowedBooksCount = 0,
                BorrowedBooks = []
            };
            await _memberRepository.CreateMemberAsync(newMember);
            await _userRepository.SaveChangeAsync();

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
          UserResponse userResponse =  await _userRepository.GetByIdAsync(id)
                .ContinueWith(t => _mapper.Map<UserResponse>(t.Result));
            if (userResponse is null)
                         return null;
            return userResponse;
        }

        public async Task<List<UserResponse>> GetAllUsersAsync()
        {
           return await _userRepository.GetUsersAsync()
                .ContinueWith(t => t.Result.Select(u => _mapper.Map<UserResponse>(u)).ToList());
        }
    }
}
