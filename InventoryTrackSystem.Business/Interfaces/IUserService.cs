using InventoryTrackSystem.Core.Common;
using InventoryTrackSystem.Model.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace InventoryTrackSystem.Business.Interfaces
{
    public interface IUserService : ICrudService<UserCreateDto, UserUpdateDto, UserGetDto, long>
    {
        Task<ResponseModel<UserGetDto>> GetByEmailAsync(string email);
        Task<ResponseModel<UserGetDto>> SignInAsync(string email, string password);
        Task<ResponseModel<UserGetDto>> ChangePasswordAsync(string token, string newPassword, CancellationToken cancellationToken = default);
        Task<ResponseModel<UserGetDto>> ResetPasswordRequestAsync(string email, CancellationToken cancellationToken = default);
    }
}
