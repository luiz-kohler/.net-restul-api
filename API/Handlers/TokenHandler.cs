using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace API.Handlers
{
    public interface ITokenHandler
    {
        string GenerateAccessToken(User user);
    }

    public class TokenHandler : ITokenHandler
    {
        private readonly string _secret;

        public TokenHandler(IOptions<TokenHandlerOptions> options)
        {
            _secret = options?.Value?.JWTSecret ?? throw new ArgumentException("JWT Secret must be informed.");
        }

        public string GenerateAccessToken(User user)
        {
            return new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(Encoding.ASCII.GetBytes(_secret))
                .AddClaim(Claims.EXPIRATION_TIME, DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds())
                .AddClaim(Claims.ID, user.Id)
                .AddClaim(Claims.NAME, user.Name)
                .AddClaim(Claims.ROLE, user.IsAdmin
                                  ? Roles.ADMIN
                                  : Roles.NON_ADMIN)
                .Encode();
        }
    }

    public static class Roles
    {
        public const string ADMIN = "admin";
        public const string NON_ADMIN = "non-admin";
    }

    public static class Claims
    {
        public const string EXPIRATION_TIME = "exp";
        public const string ID = "id";
        public const string NAME = "name";
        public const string ROLE = "role";
    }

    public class TokenHandlerOptions
    {
        public required string JWTSecret { get; set; }
    }

    public static class JWTConfigurator
    {
        public static void ConfigureJWT(this IServiceCollection services, string secret)
        {
            if (secret == null)
                throw new ArgumentException("JWT Secret must be informed.");

            var key = Encoding
                .ASCII
                .GetBytes(secret);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
    }
}
