using Abp;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.UI;
using Yootek.Authentication.External;
using Yootek.Authentication.JwtBearer;
using Yootek.Authorization;
using Yootek.Authorization.Roles;
using Yootek.Authorization.Users;
using Yootek.Configuration;
using Yootek.Identity;
using Yootek.Models.TokenAuth;
using Yootek.MultiTenancy;
using Yootek.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using NUglify.Helpers;


namespace Yootek.Controllers
{

    [Route("api/[controller]/[action]")]
    [Audited]
    public class TokenAuthController : YootekControllerBase
    {
        private readonly LogInManager _logInManager;
        private readonly UserManager _userManager;
        // private readonly SignInManager _signInManager;
        // private readonly TenantManager _tenantManager;
        private readonly ITenantCache _tenantCache;
        // private readonly IWebUrlService _webUrlService;
        private readonly ICacheManager _cacheManager;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly TokenAuthConfiguration _configuration;
        // private readonly IExternalAuthConfiguration _externalAuthConfiguration;
        // private readonly IExternalAuthManager _externalAuthManager;
        // private readonly UserRegistrationManager _userRegistrationManager;
        private readonly IdentityOptions _identityOptions;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly ISettingManager _settingManager;
        private readonly IOptions<JwtBearerOptions> _jwtOptions;
        // private readonly IJwtSecurityStampHandler _securityStampHandler;
        private readonly AbpUserClaimsPrincipalFactory<User, Role> _claimsPrincipalFactory;

        public TokenAuthController(
            LogInManager logInManager,
            UserManager userManager,
            // TenantManager tenantManager,
            ITenantCache tenantCache,
            // IWebUrlService webUrlService,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            TokenAuthConfiguration configuration,
            // IExternalAuthConfiguration externalAuthConfiguration,
            // IExternalAuthManager externalAuthManager,
            IOptions<IdentityOptions> identityOptions,
            // SignInManager signInManager,
            IAppConfigurationAccessor configurationAccessor,
            ISettingManager settingManager,
            // UserRegistrationManager userRegistrationManager,
            // IJwtSecurityStampHandler securityStampHandler,
            IOptions<JwtBearerOptions> jwtOptions,
            AbpUserClaimsPrincipalFactory<User, Role> claimsPrincipalFactory,
            ICacheManager cacheManager
            )
        {
            _logInManager = logInManager;
            _userManager = userManager;
            // _tenantManager = tenantManager;
            _tenantCache = tenantCache;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _configuration = configuration;
            // _externalAuthConfiguration = externalAuthConfiguration;
            // _externalAuthManager = externalAuthManager;
            // _userRegistrationManager = userRegistrationManager;
            // _signInManager = signInManager;
            // _webUrlService = webUrlService;
            _identityOptions = identityOptions.Value;
            _appConfiguration = configurationAccessor.Configuration;
            _settingManager = settingManager;
            _jwtOptions = jwtOptions;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _cacheManager = cacheManager;
            // _securityStampHandler = securityStampHandler;
        }

        [HttpPost]     
        public async Task<AuthenticateResultModel> Authenticate([FromBody] AuthenticateModelTenant model)
        {
            try
            {
                Logger.Fatal(JsonConvert.SerializeObject(model));
                long t1 = TimeUtils.GetNanoseconds();


                var mobileSetting = _appConfiguration["MobileVersion"] ?? "0.0.2";
                var loginResult = await GetLoginResultAsync(
                    model.UserNameOrEmailAddress,
                    model.Password,
                    !string.IsNullOrEmpty(model.TenancyName) ? model.TenancyName : GetTenancyNameOrNull()
                );

                var vrs = new
                {
                    appVersion = new
                    {
                        version = mobileSetting,
                        typeVersion = 1
                    }

                };
                var mobileConfig = JsonConvert.SerializeObject(vrs);
                var claimToken = await CreateJwtClaims(loginResult.Identity, loginResult.User,
                    tokenType: TokenType.RefreshToken);

                var refreshToken = CreateRefreshToken(claimToken);
                var claimRefresh = await CreateJwtClaims(loginResult.Identity, loginResult.User,
                  refreshTokenKey: refreshToken.key);
                var accessToken = CreateAccessToken(claimRefresh);
                mb.statisticMetris(t1, 0, "authenticate");
                var result = new AuthenticateResultModel
                {
                    AccessToken = accessToken,
                    EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                    ExpireInSeconds = (int)_configuration.AccessTokenExpiration.TotalSeconds,
                    UserId = loginResult.User.Id,
                    EmailAddress = loginResult.User.EmailAddress,
                    TenantId = loginResult.Tenant != null ? loginResult.Tenant.Id : 0,
                    ThirdAccounts = loginResult.User.ThirdAccounts,
                    MobileConfig = loginResult.Tenant != null ? loginResult.Tenant.MobileConfig : mobileConfig,
                    RefreshToken = refreshToken.token,
                    RefreshTokenExpireInSeconds = (int)_configuration.RefreshTokenExpiration.TotalSeconds,
                };

                Logger.Fatal(JsonConvert.SerializeObject(result));
                return result;
            }
            catch(UserFriendlyException e)
            {
                Logger.Fatal(e.Message, e);
                throw;
            }
        }

        [HttpPost]
        public async Task<RefreshTokenResult> RefreshToken(string refreshToken)
        {
            Logger.Fatal(refreshToken);
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            if (!IsRefreshTokenValid(refreshToken, out var principal))
            {
                throw new UserFriendlyException("Refresh token is not valid!");
            }

            try
            {
                var user = await _userManager.GetUserAsync(
                    UserIdentifier.Parse(principal.Claims.First(x => x.Type == AppConsts.UserIdentifier).Value)
                );

                if (user == null)
                {
                    throw new UserFriendlyException("Unknown user or user identifier");
                }

                principal = await _claimsPrincipalFactory.CreateAsync(user);

                var accessToken = CreateAccessToken(await CreateJwtClaims(principal.Identity as ClaimsIdentity, user));

                var result = new RefreshTokenResult(
                    accessToken,
                    GetEncryptedAccessToken(accessToken),
                    (int)_configuration.AccessTokenExpiration.TotalSeconds);
                Logger.Fatal(JsonConvert.SerializeObject(result));

                return await Task.FromResult(result);
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ValidationException("Refresh token is not valid!", e);
            }
        }


        [HttpGet]
        [AbpAuthorize]
        public async Task LogOut()
        {
            if (AbpSession.UserId != null)
            {

                var tokenValidityKeyInClaims = User.Claims.ToList().First(c => c.Type == AppConsts.TokenValidityKey);
                await RemoveTokenAsync(tokenValidityKeyInClaims.Value);

                var refreshTokenValidityKeyInClaims =
                    User.Claims.FirstOrDefault(c => c.Type == AppConsts.RefreshTokenValidityKey);
                if (refreshTokenValidityKeyInClaims != null)
                {
                    await RemoveTokenAsync(refreshTokenValidityKeyInClaims.Value);
                }
            }
        }

        private bool AllowOneConcurrentLoginPerUser()
        {
            return _settingManager.GetSettingValue<bool>(AppSettings.UserManagement.AllowOneConcurrentLoginPerUser);
        }

        private async Task RemoveTokenAsync(string tokenKey)
        {
            await _userManager.RemoveTokenValidityKeyAsync(
                await _userManager.GetUserAsync(AbpSession.ToUserIdentifier()), tokenKey
            );

            await _cacheManager.GetCache(AppConsts.TokenValidityKey).RemoveAsync(tokenKey);
        }
        

        private async Task<IEnumerable<Claim>> CreateJwtClaims(
         ClaimsIdentity identity, User user,
         TimeSpan? expiration = null,
         TokenType tokenType = TokenType.AccessToken,
         string refreshTokenKey = null)
        {
            var tokenValidityKey = Guid.NewGuid().ToString();
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == _identityOptions.ClaimsIdentity.UserIdClaimType);

            if (_identityOptions.ClaimsIdentity.UserIdClaimType != JwtRegisteredClaimNames.Sub)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value));
            }

            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64),
                new Claim(AppConsts.TokenValidityKey, tokenValidityKey),
                new Claim(AppConsts.UserIdentifier, user.ToUserIdentifier().ToUserIdentifierString()),
                new Claim(AppConsts.TokenType, tokenType.To<int>().ToString())
            });

            if (!string.IsNullOrEmpty(refreshTokenKey))
            {
                claims.Add(new Claim(AppConsts.RefreshTokenValidityKey, refreshTokenKey));
            }

            if (!expiration.HasValue)
            {
                expiration = tokenType == TokenType.AccessToken
                    ? _configuration.AccessTokenExpiration
                    : _configuration.RefreshTokenExpiration;
            }

            var expirationDate = DateTime.UtcNow.Add(expiration.Value);

            await _cacheManager
                .GetCache(AppConsts.TokenValidityKey)
                .SetAsync(tokenValidityKey, "", absoluteExpireTime: new DateTimeOffset(expirationDate));

            using (CurrentUnitOfWork.SetTenantId(user.TenantId))
            {
                await _userManager.AddTokenValidityKeyAsync(
                user,
                tokenValidityKey,
                expirationDate
                );
            }

            return claims;
        }
        
        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(expiration ?? _configuration.AccessTokenExpiration),
                signingCredentials: _configuration.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private static List<Claim> CreateJwtClaims(ClaimsIdentity identity)
        {
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

            return claims;
        }

        private string GetEncryptedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);
        }

        private (string token, string key) CreateRefreshToken(IEnumerable<Claim> claims)
        {
            var claimsList = claims.ToList();
            return (CreateToken(claimsList, AppConsts.RefreshTokenExpiration),
                claimsList.First(c => c.Type == AppConsts.TokenValidityKey).Value);
        }

        private string CreateToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                signingCredentials: _configuration.SigningCredentials,
                expires: expiration == null ? (DateTime?)null : now.Add(expiration.Value)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }


        private bool IsRefreshTokenValid(string refreshToken, out ClaimsPrincipal principal)
        {
            principal = null;

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidAudience = _configuration.Audience,
                    ValidIssuer = _configuration.Issuer,
                    IssuerSigningKey = _configuration.SecurityKey
                };

                foreach (var validator in _jwtOptions.Value.SecurityTokenValidators)
                {
                    if (!validator.CanReadToken(refreshToken))
                    {
                        continue;
                    }

                    try
                    {
                        principal = validator.ValidateToken(refreshToken, validationParameters, out _);

                        if (principal.Claims.FirstOrDefault(x => x.Type == AppConsts.TokenType)?.Value ==
                            TokenType.RefreshToken.To<int>().ToString())
                        {
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Debug(ex.ToString(), ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Debug(ex.ToString(), ex);
            }

            return false;
        }

    }
}
