using InventoryTrackSystem.Core.Common;
using InventoryTrackSystem.Model.Dtos.Auth;

namespace InventoryTrackSystem.Business.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseModel<CurrentUserDto>> MeAsync(); 
        Task<ResponseModel<AuthResponseDto>> LoginAsync(LoginRequestDto loginRequest, CancellationToken ct = default);
        Task<ResponseModel<RegisterResponseDto>> RegisterAsync(RegisterRequestDto registerRequest, CancellationToken ct = default);
    }
}
