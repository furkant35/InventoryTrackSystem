using InventoryTrackSystem.Business.Interfaces;
using InventoryTrackSystem.Core.Common;
using InventoryTrackSystem.Core.Enums;
using InventoryTrackSystem.Core.Settings.Concrete;
using InventoryTrackSystem.Core.Utilities.Constants;
using InventoryTrackSystem.Model.Dtos.Auth;
using InventoryTrackSystem.Model.Dtos.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InventoryTrackSystem.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _http;
        private readonly IUserService _userService;
        private readonly IOptionsSnapshot<AppSettings> _appSettings;

        public AuthService(
            IHttpContextAccessor http,
            IUserService userService,
            IOptionsSnapshot<AppSettings> appSettings
        )
        {
            _http = http;
            _userService = userService;
            _appSettings = appSettings;
        }

        public async Task<ResponseModel<AuthResponseDto>> LoginAsync(LoginRequestDto loginRequest, CancellationToken ct = default)
        {
            // Kullanıcı doğrulama
            var result = await _userService.SignInAsync(loginRequest.Email, loginRequest.Password);
            if (!result.IsSuccess || result.Data == null || !result.Data.IsActive)
            {
                return ResponseModel<AuthResponseDto>.Fail(Messages.InvalidEmailOrPassword, StatusCode.Unauthorized);
            }

            var user = result.Data;

            // AppSettings
            var issuer = _appSettings.Value.Issuer;
            var audience = _appSettings.Value.Audience;
            var key = _appSettings.Value.Key;
            var minutes = _appSettings.Value.AccessTokenMinutes > 0 ? _appSettings.Value.AccessTokenMinutes : 60;

            // Claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (!string.IsNullOrWhiteSpace(user.Email))
                claims.Add(new Claim(ClaimTypes.Email, user.Email));

            // Token oluştur
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(minutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var dto = new AuthResponseDto
            {
                Token = tokenString,
                Expires = expires,
                User = user,
                Status = 200
            };

            return ResponseModel<AuthResponseDto>.Success(dto, Messages.SignInSuccessful);
        }

        public async Task<ResponseModel<CurrentUserDto>> MeAsync()
        {
            var p = _http.HttpContext?.User;
            var isAuth = p?.Identity?.IsAuthenticated ?? false;

            if (!isAuth)
                return ResponseModel<CurrentUserDto>.Success(new CurrentUserDto { IsAuthenticated = false });

            var idStr = p?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? p?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (!long.TryParse(idStr, out var userId) || userId <= 0)
                return ResponseModel<CurrentUserDto>.Fail(Messages.UserIdClaimNotFound, StatusCode.Unauthorized);

            var userRes = await _userService.GetByIdAsync(userId);
            if (!userRes.IsSuccess || userRes.Data is null)
                return ResponseModel<CurrentUserDto>.Fail(Messages.UserNotFound, StatusCode.NotFound);

            var u = userRes.Data;

            var dto = new CurrentUserDto
            {
                IsAuthenticated = true,
                Id = u.Id,
                Name = u.Name,
                Surname = u.Surname,
                Email = u.Email,
                Phone = u.Phone
            };

            return ResponseModel<CurrentUserDto>.Success(dto);
        }

        public async Task<ResponseModel<RegisterResponseDto>> RegisterAsync(RegisterRequestDto registerRequest, CancellationToken ct = default)
        {
            try
            {
                var existing = await _userService.GetByEmailAsync(registerRequest.Email);


                if (existing.Data != null)
                    return ResponseModel<RegisterResponseDto>.Fail(Messages.RegisterEmailExists, StatusCode.Conflict);

                var createResult = await _userService.CreateAsync(new UserCreateDto
                {
                    Name = registerRequest.Name,
                    Surname = registerRequest.Surname,
                    Email = registerRequest.Email,
                    Phone = registerRequest.Phone,
                    Password = registerRequest.Password
                });

                if (!createResult.IsSuccess || createResult.Data == null)
                    return ResponseModel<RegisterResponseDto>.Fail(Messages.RegisterFailed, StatusCode.Error);

                var user = createResult.Data;

                // 3️⃣ Token üret (otomatik login)
                var issuer = _appSettings.Value.Issuer;
                var audience = _appSettings.Value.Audience;
                var key = _appSettings.Value.Key;
                var minutes = _appSettings.Value.AccessTokenMinutes > 0 ? _appSettings.Value.AccessTokenMinutes : 60;

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                if (!string.IsNullOrWhiteSpace(user.Email))
                    claims.Add(new Claim(ClaimTypes.Email, user.Email));

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.UtcNow.AddMinutes(minutes);

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                var response = new RegisterResponseDto
                {
                    Expires = expires,
                    Token = tokenString,
                    User = user
                };

                return ResponseModel<RegisterResponseDto>.Success(response, Messages.RegisterSuccess);
            }
            catch (Exception ex)
            {
                return ResponseModel<RegisterResponseDto>.Fail($"{Messages.RegisterFailed}: {ex.Message}", StatusCode.Error);
            }
        }
    }
}
