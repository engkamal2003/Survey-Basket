using SurveyBasket.Api.Authentication;

namespace SurveyBasket.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtProvider _jwtProvider;

        public AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider)
        {
            _userManager = userManager;
            _jwtProvider = jwtProvider;
        }

        public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return null;

            var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!isValidPassword)
                return null;

            var (token, expiresIn) = _jwtProvider.GenerateToken(user);
            return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn);
        }
    }
}
