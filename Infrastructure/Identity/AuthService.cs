using Application.Common.Interfaces;
using Application.Common.Models;
using Azure.Core;
using Domain.Identity;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Progressly.Application.DocumentStore.Commands.UploadFile;
using Progressly.Application.DocumentStore.DTOs;
using Progressly.Application.DocumentStore.Queries.GetFile;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infrastructure.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly GetFileQueryHandler _getFileQueryHandler;
        private readonly HttpContextAccessor _httpContextAccessor;
        private readonly UploadFileCommandHandler _uploadFileCommandHandler;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICurrentUserService _currentUserService;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration,GetFileQueryHandler getFileQueryHandler
            ,HttpContextAccessor httpContextAccessor,UploadFileCommandHandler uploadFileCommandHandler,RoleManager<IdentityRole> roleManager,ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _getFileQueryHandler = getFileQueryHandler;
            _httpContextAccessor = httpContextAccessor;
            _uploadFileCommandHandler = uploadFileCommandHandler;
            _roleManager = roleManager;
            _currentUserService = currentUserService;
        }

        public async Task<Response<Domain.Identity.AuthResponse>> RegisterAsync(Domain.Identity.RegisterRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName=request.FirstName.Trim()+" "+request.LastName.Trim(),
                IsActive=request.IsActive
            };

            var result = await _userManager.CreateAsync(user, request.Password ?? "");
            if (!result.Succeeded)
            {
                var auth = new AuthResponse
                {
                    IsAuthenticated = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
                return new Response<AuthResponse> { result = Result.Failure(auth.Errors) };
            }
            var roleToAssign = string.IsNullOrWhiteSpace(request.Role) ? "USER" : request.Role;
            var roleExists = await _roleManager.RoleExistsAsync(roleToAssign);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleToAssign));
            }
            var roleResult = await _userManager.AddToRoleAsync(user, roleToAssign);
            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors.Select(e => e.Description).ToList();
                return new Response<AuthResponse> { result = Result.Failure(errors) };
            }
            return await GenerateJwtToken(user);
        }



        public async Task<Response<Domain.Identity.AuthResponse>> LoginAsync(Domain.Identity.LoginRequest request)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == request.UserId || x.PhoneNumber == request.UserId);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password ?? ""))
            {
                return new Response<AuthResponse>() { result = Result.Failure(new List<string> { "Invalid credentials" }) };
            }
            if (user.IsActive==false)
            {
                return new Response<AuthResponse>
                {
                    result = Result.Failure(new List<string> { "Your account is inactive. Please contact the administrator." })
                };
            }
            return await GenerateJwtToken(user);
        }

        private async Task<Response<Domain.Identity.AuthResponse>> GenerateJwtToken(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.NameIdentifier, user.Id ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"] ?? ""));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:DurationInMinutes"])),
                claims: authClaims,
                signingCredentials: credentials
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            var authResponse = new Domain.Identity.AuthResponse
            {
                IsAuthenticated = true,
                Token = jwtToken,
                Email = user.Email,
                UserName = string.IsNullOrWhiteSpace(user.FullName)?user.Email: user.FullName,
                UserId=user.Id
            };

            return new Response<Domain.Identity.AuthResponse>
            {
                result = Result.Success(),
                data = authResponse
            };
        }

        public async Task<Response<bool>> UpdateUserAsync(UpdateUserRequest request)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var user = httpContext?.User;
                var userId="";
                if (!string.IsNullOrWhiteSpace(request.UserId))
                {
                    userId = request.UserId;
                }
                else
                {
                 userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                }

                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new Response<bool>
                    {
                        result = Result.Failure(new List<string> { "UserId not found!" })
                    };
                }

                var existingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (existingUser == null)
                {
                    return new Response<bool>
                    {
                        result = Result.Failure(new List<string> { "User not found!" })
                    };
                }
                if (request.IsActive!=null)
                {
                    existingUser.IsActive = request.IsActive;
                }
                // Update user fields
                if (!string.IsNullOrWhiteSpace(request.FullName))
                    existingUser.FullName = request.FullName;

                if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                    existingUser.PhoneNumber = request.PhoneNumber;
                
                if (request.ProfileImage!=null)
                {
                    var fileResult = await _uploadFileCommandHandler.HandleAsync(new UploadFileCommand { File = request.ProfileImage });
                    if (fileResult.Item1 == null)
                    {
                        return new Response<bool>() { result = Result.Failure(new List<string>() { "File upload failed!" }) };
                    }
                    existingUser.ProfilePath = fileResult.Item2 ?? "";
                    existingUser.ProfileName = fileResult.Item1;
                }

                if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != existingUser.Email)
                {
                    var setEmailResult = await _userManager.SetEmailAsync(existingUser, request.Email);
                    if (!setEmailResult.Succeeded)
                    {
                        return new Response<bool>
                        {
                            result = Result.Failure(setEmailResult.Errors.Select(e => e.Description).ToList())
                        };
                    }

                    var setUserNameResult = await _userManager.SetUserNameAsync(existingUser, request.Email);
                    if (!setUserNameResult.Succeeded)
                    {
                        return new Response<bool>
                        {
                            result = Result.Failure(setUserNameResult.Errors.Select(e => e.Description).ToList())
                        };
                    }
                }

                if (!string.IsNullOrEmpty(request.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                    var passwordResult = await _userManager.ResetPasswordAsync(existingUser, token, request.Password);
                   
                }


                var updateResult = await _userManager.UpdateAsync(existingUser);
                if (!updateResult.Succeeded)
                {
                    return new Response<bool>
                    {
                        result = Result.Failure(updateResult.Errors.Select(e => e.Description).ToList())
                    };
                }

                return new Response<bool>
                {
                    result = Result.Success(),
                    data = true
                };
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message ?? ex.ToString();
                return new Response<bool>
                {
                    result = Result.Failure(new List<string> { message })
                };
            }
        }


        public async Task<Response<FileInfoDto>> ProfileImageAsync()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var user = httpContext?.User;
                var userId = user?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "";

                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new Response<FileInfoDto>() { result = Result.Failure(new List<string>() { "UserId Not Found!" }) };
                }
                var isUserExisted = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (isUserExisted == null)
                {
                    return new Response<FileInfoDto>() { result = Result.Failure(new List<string>() { "User Not Found!" }) };
                }
                var originalFileName = isUserExisted.ProfileName;
                var displayFileName = string.IsNullOrWhiteSpace(originalFileName) || !originalFileName.Contains('-')
                    ? originalFileName ?? ""
                    : originalFileName.Substring(originalFileName.IndexOf('-') + 1);

                var fileInfoDTO = await _getFileQueryHandler.Handle(new GetFileQuery() { FilePath = isUserExisted.ProfilePath ?? "", FileName = displayFileName ?? "" });
                if (fileInfoDTO == null)
                {
                    return new Response<FileInfoDto>() { result = Result.Failure(new List<string>() { "File not exist!" }) };
                }
                return new Response<FileInfoDto>() { result = Result.Success(), data = fileInfoDTO };
            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<FileInfoDto>() { result = Result.Failure(new List<string>() { message }) };
            }
        }

        public async Task<Response<List<AdminUserDTO>>> GetAdminUsers()
        {
            var users = await _userManager.Users.ToListAsync();
           
            var adminUsers = new List<AdminUserDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("ADMIN"))
                {
                    adminUsers.Add(new AdminUserDTO
                    {
                        UserName = user.FullName,
                        UserId = user.Id,
                        Email = user.Email,
                        IsActive = user.IsActive
                    });
                }
            }
            if (adminUsers.Count==0||adminUsers==null)
            {
                return new Response<List<AdminUserDTO>>() { result = Result.Failure(new List<string> { "No Data Found" }) };
            }

            return new Response<List<AdminUserDTO>>()
            {
                result = Result.Success(),
                data = adminUsers
            };
        }

        public async Task<Response<List<AdminUserDTO>>> GetAllActiveUsers()
        {
            var users = await _userManager.Users.Where(x=>x.IsActive==true).ToListAsync();
            var allUsers = new List<AdminUserDTO>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains("SUPER_ADMIN"))
                {
                    allUsers.Add(new AdminUserDTO
                    {
                        UserName = user.FullName,
                        UserId = user.Id,
                        Email = user.Email,
                        Role=roles.FirstOrDefault(),
                        IsActive = user.IsActive
                    });
                }
            }
            if (users.Count == 0 || users == null)
            {
                return new Response<List<AdminUserDTO>>() { result = Result.Failure(new List<string> { "No Data Found" }) };
            }

            return new Response<List<AdminUserDTO>>()
            {
                result = Result.Success(),
                data = allUsers
            };
        }
        public async Task<Response<int>> GetAllActiveUsersCount()
        {
            var users = await _userManager.Users.Where(x => x.IsActive==true).ToListAsync();
            int count = 0;

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains("SUPER_ADMIN"))
                {
                    count++;
                }
            }

            if (count == 0)
            {
                return new Response<int>()
                {
                    result = Result.Failure(new List<string> { "No Active Users Found" })
                };
            }

            return new Response<int>()
            {
                result = Result.Success(),
                data = count
            };
        }


        public async Task<Response<UserDTO>> GetUserInfo()
        {
            try
            {
                var userId = _currentUserService.UserId;
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new Response<UserDTO>() { result = Result.Failure(new List<string> { "UserId Not Found" }) };
                }
                var user = await _userManager.FindByIdAsync(userId);
                if (user==null)
                {
                    return new Response<UserDTO>() { result = Result.Failure(new List<string> { "User Not Found" }) };
                }
                var userDetail = new UserDTO
                {
                    Email = user.Email,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber
                };
                return new Response<UserDTO>() { result = Result.Success(), data = userDetail };
            }
            catch(Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<UserDTO>() { result = Result.Failure(new List<string>() { message }) };
            }
        }


    }
}
