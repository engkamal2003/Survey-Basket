﻿//using System.Security.Cryptography;
//using SurveyBasket.Api.Authentication;
//using SurveyBasket.Api.Errors;

//namespace SurveyBasket.Api.Services
//{
//    public class AuthService : IAuthService
//    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly IJwtProvider _jwtProvider;
//        private readonly int _refreshTokenExpiryDays = 14;

//        public AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider)
//        {
//            _userManager = userManager;
//            _jwtProvider = jwtProvider;
//        }

//        public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
//        {
//            var user = await _userManager.FindByEmailAsync(email);
//            if (user is null)
//                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

//            var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
//            if (!isValidPassword)
//                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

//            var (token, expiresIn) = _jwtProvider.GenerateToken(user);

//            var refreshToken = GenerateRefreshToken();
//            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

//            user.RefreshTokens.Add( new RefreshToken
//                {
//                     Token = refreshToken,
//                     ExpiresOn = refreshTokenExpiration
//                });

//            await _userManager.UpdateAsync(user);

//            var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration);

//            return Result.Success(response);
//        }

//        public async Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
//        {
//            var userId = _jwtProvider.ValidateToken(token);

//            if (userId is null)
//                return null;

//            var user = await _userManager.FindByIdAsync(userId);

//            if (user is null)
//                return null;

//            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

//            if (userRefreshToken is null)
//                return null;

//            userRefreshToken.RevokedOn = DateTime.UtcNow;

//            var (newToken, expiresIn) = _jwtProvider.GenerateToken(user);

//            var newRefreshToken = GenerateRefreshToken();
//            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

//            user.RefreshTokens.Add(new RefreshToken
//            {
//                Token = newRefreshToken,
//                ExpiresOn = refreshTokenExpiration
//            });

//            await _userManager.UpdateAsync(user);

//            return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpiration);
//        }

//        public async Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
//        {
//            var userId = _jwtProvider.ValidateToken(token);

//            if (userId is null)
//                return false;

//            var user = await _userManager.FindByIdAsync(userId);

//            if (user is null)
//                return false;

//            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

//            if (userRefreshToken is null)
//                return false;

//            userRefreshToken.RevokedOn = DateTime.UtcNow;

//            await _userManager.UpdateAsync(user);

//            return true;
//        }

//        private static string GenerateRefreshToken()
//        {
//            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
//        }
//    }
//}


//using Microsoft.AspNetCore.Identity;
//using OneOf;
//using SurveyBasket.Api.Authentication;
//using SurveyBasket.Api.Errors;
//using System.Security.Cryptography;
//using System.Threading;
//using System.Threading.Tasks;


//namespace SurveyBasket.Services
//{
//    public class AuthService : IAuthService
//    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly IJwtProvider _jwtProvider;
//        private readonly int _refreshTokenExpiryDays;

//        public AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider)
//        {
//            _userManager = userManager;
//            _jwtProvider = jwtProvider;
//            _refreshTokenExpiryDays = 14;
//        }

//        public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
//        {
//            var user = await _userManager.FindByEmailAsync(email);

//            if (user == null)
//                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

//            var isValidPassword = await _userManager.CheckPasswordAsync(user, password);

//            if (!isValidPassword)
//                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

//            var (token, expiresIn) = _jwtProvider.GenerateToken(user);
//            var refreshToken = GenerateRefreshToken();
//            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

//            user.RefreshTokens.Add(new RefreshToken
//            {
//                Token = refreshToken,
//                ExpiresOn = refreshTokenExpiration
//            });

//            await _userManager.UpdateAsync(user);

//            var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration);

//            return Result.Success(response);
//        }

//        public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
//        {
//            var userId = _jwtProvider.ValidateToken(token);

//            if (userId == null)
//                return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

//            var user = await _userManager.FindByIdAsync(userId);

//            if (user == null)
//                return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

//            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

//            if (userRefreshToken == null)
//                return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

//            userRefreshToken.RevokedOn = DateTime.UtcNow;

//            var (newToken, expiresIn) = _jwtProvider.GenerateToken(user);
//            var newRefreshToken = GenerateRefreshToken();
//            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

//            user.RefreshTokens.Add(new RefreshToken
//            {
//                Token = newRefreshToken,
//                ExpiresOn = refreshTokenExpiration
//            });

//            await _userManager.UpdateAsync(user);

//            var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpiration);

//            return Result.Success(response);
//        }

//        public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
//        {
//            var userId = _jwtProvider.ValidateToken(token);

//            if (userId == null)
//                return Result.Failure(UserErrors.InvalidJwtToken);

//            var user = await _userManager.FindByIdAsync(userId);

//            if (user == null)
//                return Result.Failure(UserErrors.InvalidJwtToken);

//            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

//            if (userRefreshToken == null)
//                return Result.Failure(UserErrors.InvalidRefreshToken);

//            userRefreshToken.RevokedOn = DateTime.UtcNow;

//            await _userManager.UpdateAsync(user);

//            return Result.Success();
//        }

//        private static string GenerateRefreshToken()
//        {
//            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
//        }
//    }
//}








using Microsoft.AspNetCore.Identity;
using OneOf;
using SurveyBasket.Api.Authentication;
using SurveyBasket.Api.Errors;
using System.Security.Cryptography;

namespace SurveyBasket.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtProvider _jwtProvider;
        private readonly int _refreshTokenExpiryDays = 14;

        public AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider)
        {
            _userManager = userManager;
            _jwtProvider = jwtProvider;
        }

        public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

            var isValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!isValidPassword)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

            var (token, expiresIn) = _jwtProvider.GenerateToken(user);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenExpiration
            });

            await _userManager.UpdateAsync(user);

            var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration);

            return Result.Success(response);
        }

        // Uncomment if needed
        // public async Task<OneOf<AuthResponse, Error>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        // {
        //     var user = await _userManager.FindByEmailAsync(email);

        //     if (user is null)
        //         return UserErrors.InvalidCredentials;

        //     var isValidPassword = await _userManager.CheckPasswordAsync(user, password);

        //     if (!isValidPassword)
        //         return UserErrors.InvalidCredentials;

        //     var (token, expiresIn) = _jwtProvider.GenerateToken(user);
        //     var refreshToken = GenerateRefreshToken();
        //     var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        //     user.RefreshTokens.Add(new RefreshToken
        //     {
        //         Token = refreshToken,
        //         ExpiresOn = refreshTokenExpiration
        //     });

        //     await _userManager.UpdateAsync(user);

        //     return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration);
        // }

        public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token);

            if (userId is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

            if (userRefreshToken is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            var (newToken, expiresIn) = _jwtProvider.GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefreshToken,
                ExpiresOn = refreshTokenExpiration
            });

            await _userManager.UpdateAsync(user);

            var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpiration);

            return Result.Success(response);
        }

        public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token);

            if (userId is null)
                return Result.Failure(UserErrors.InvalidJwtToken);

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure(UserErrors.InvalidJwtToken);

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

            if (userRefreshToken is null)
                return Result.Failure(UserErrors.InvalidRefreshToken);

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return Result.Success();
        }

        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
