using Personal_Blogging_Platform_API.DTOs.Auth;

namespace Personal_Blogging_Platform_API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseDto> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
    }
}
