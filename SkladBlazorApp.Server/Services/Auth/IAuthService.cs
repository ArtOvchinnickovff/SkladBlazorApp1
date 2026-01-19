using SkladBlazorApp.Shared.ModelsDTO;

namespace SkladBlazorApp.Server.Services.Auth
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginDto dto);
    }
}
