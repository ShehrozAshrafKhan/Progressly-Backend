using Application.Common.Models;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity.Data;
using Progressly.Application.DocumentStore.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public interface IAuthService
    {
        Task<Response<Domain.Identity.AuthResponse>> RegisterAsync(Domain.Identity.RegisterRequest request);
        Task<Response<Domain.Identity.AuthResponse>> LoginAsync(Domain.Identity.LoginRequest request);
        Task<Response<bool>> UpdateUserAsync(Domain.Identity.UpdateUserRequest request);
        Task<Response<FileInfoDto>> ProfileImageAsync();
        Task<Response<UserDTO>> GetUserInfo();
        Task<Response<List<AdminUserDTO>>> GetAdminUsers();
        Task<Response<List<AdminUserDTO>>> GetAllActiveUsers();
        Task<Response<int>> GetAllActiveUsersCount();
    }
}
