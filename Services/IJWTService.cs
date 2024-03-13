using Microsoft.IdentityModel.Tokens;

namespace JWT_Example.Services
{
    public interface IJWTService
    {
        string GenerateJwtToken(string audience);
        Task<TokenValidationResult> ValidateJwtToken(string inputToken, string audience);
    }
}
