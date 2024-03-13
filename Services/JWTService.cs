using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;
using System.Configuration;
using JWT_Example.Config;
using Microsoft.Extensions.Options;

namespace JWT_Example.Services
{
    public class JWTService : IJWTService
    {
        private readonly JsonWebTokenHandler _jwtSecurityTokenHandler;
        private readonly IApiConfig _configuration;

        public JWTService(IOptions<ApiConfig> configuration) { 
            _configuration = configuration.Value;
            _jwtSecurityTokenHandler = new JsonWebTokenHandler();
        }
        public string GenerateJwtToken(string? audience)
        {
            string encryptionAlgorithm = SecurityAlgorithms.HmacSha256Signature;
            string? JWTSecretKey = _configuration.JWTSecretKey;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSecretKey));
            var signingCredentials = new SigningCredentials(securityKey, encryptionAlgorithm);
            _ = double.TryParse(_configuration.JWTLifeTimeInMinutes, out double tokenExpiry);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Subject = new ClaimsIdentity(payloadClaims),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Role, _configuration.JWTClaimRole)
                }),
                Expires = DateTime.UtcNow.AddMinutes(tokenExpiry),
                SigningCredentials = signingCredentials,
                Issuer = _configuration.JWTIssuer,
                Audience = audience
            };
            return _jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
        }

        public Task<TokenValidationResult> ValidateJwtToken(string inputToken, string audience)
        {
            string JWTSecretKey = _configuration.JWTSecretKey;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSecretKey));
            var validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = securityKey,
                ValidIssuer = _configuration.JWTIssuer,
                ValidAudience = audience
                //ValidateActor = true
            };
            return _jwtSecurityTokenHandler.ValidateTokenAsync(inputToken, validationParameters);
        }
    }
}
