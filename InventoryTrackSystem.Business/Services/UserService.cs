using Business.Services.Base;
using InventoryTrackSystem.Business.Interfaces;
using InventoryTrackSystem.Business.UnitOfWork;
using InventoryTrackSystem.Core.Common;
using InventoryTrackSystem.Core.Enums;
using InventoryTrackSystem.Core.Utilities.Constants;
using InventoryTrackSystem.Model.Concrete;
using InventoryTrackSystem.Model.Dtos.User;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InventoryTrackSystem.Business.Services
{
    public class UserService
        : CrudServiceBase<User, long, UserCreateDto, UserUpdateDto, UserGetDto>, IUserService
    {
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(
            IUnitOfWork uow,
            IMapper mapper,
            TypeAdapterConfig config,
            IPasswordHasher<User> passwordHasher
        ) : base(uow, mapper, config)
        {
            _passwordHasher = passwordHasher;
        }

        protected override long ReadKey(User entity) => entity.Id;
        protected override Expression<Func<User, bool>> KeyPredicate(long id) => u => u.Id == id;

        public override async Task<ResponseModel<UserGetDto>> CreateAsync(UserCreateDto dto)
        {
            try
            {
                var entity = _mapper.Map<User>(dto);

                if (string.IsNullOrWhiteSpace(dto.Password))
                    return ResponseModel<UserGetDto>.Fail(Messages.PasswordRequired, StatusCode.BadRequest);

                entity.PasswordHash = _passwordHasher.HashPassword(entity, dto.Password);

                await _unitOfWork.Repository.AddAsync(entity);
                await _unitOfWork.Repository.CompleteAsync();

                var createdDto = _mapper.Map<UserGetDto>(entity);
                return ResponseModel<UserGetDto>.Success(createdDto, Messages.Created, StatusCode.Created);
            }
            catch (DbUpdateException ex)
            {
                return ResponseModel<UserGetDto>.Fail($"{Messages.DatabaseError}: {ex.Message}", StatusCode.Conflict);
            }
            catch (Exception ex)
            {
                return ResponseModel<UserGetDto>.Fail($"{Messages.UnexpectedError}: {ex.Message}", StatusCode.Error);
            }
        }

        protected override async Task<User?> ResolveEntityForUpdateAsync(UserUpdateDto dto)
        {
            if (dto.Id <= 0) return null;

            return await _unitOfWork.Repository.GetByIdAsync<User>(
                asNoTracking: false,
                id: dto.Id
            );
        }

        protected override void MapUpdate(UserUpdateDto dto, User entity)
        {
            base.MapUpdate(dto, entity);

            //if (!string.IsNullOrWhiteSpace(dto.NewPassword))
            //    entity.PasswordHash = _passwordHasher.HashPassword(entity, dto.NewPassword);
        }

        public async Task<ResponseModel<UserGetDto>> SignInAsync(string email, string password)
        {
            var user = await _unitOfWork.Repository
                .GetQueryable<User>()
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return ResponseModel<UserGetDto>.Fail(Messages.InvalidEmailOrPassword, StatusCode.NotFound);

            var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (verify == PasswordVerificationResult.Failed)
                return ResponseModel<UserGetDto>.Fail(Messages.InvalidEmailOrPassword, StatusCode.BadRequest);

            var dto = _mapper.Map<UserGetDto>(user);
            return ResponseModel<UserGetDto>.Success(dto, Messages.SignInSuccessful, StatusCode.Ok);
        }

        public Task<ResponseModel<UserGetDto>> ChangePasswordAsync(string token, string newPassword, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<UserGetDto>> ResetPasswordRequestAsync(string email, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<UserGetDto>> GetByEmailAsync(string email)
        {
            var user = await _unitOfWork.Repository
                .GetQueryable<User>()
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return ResponseModel<UserGetDto>.Fail(Messages.UserNotFound, StatusCode.NotFound);

            var dto = _mapper.Map<UserGetDto>(user);
            return ResponseModel<UserGetDto>.Success(dto, status: StatusCode.Ok);
        }

    }
}
