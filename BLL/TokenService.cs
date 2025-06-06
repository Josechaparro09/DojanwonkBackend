using DAL.Modelos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BLL
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        // ✅ Constructor para inyección de dependencias
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // ✅ Método mejorado con configuración centralizada
        public string GenerateToken(Usuario usuario)
        {
            // ✅ MISMA CLAVE que en Program.cs (32 bytes exactos)
            var jwtKey = "mi-clave-super-secreta-de-32bytes"; // 32 caracteres
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // ✅ Claims mejorados con más información
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.UserName ?? ""),
                new Claim(ClaimTypes.Role, usuario.Rol ?? "Usuario"),
                new Claim(ClaimTypes.NameIdentifier, usuario.Cc),
                new Claim("userId", usuario.Cc), // Claim personalizado
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // ID único del token
            };

            // ✅ Token con configuración completa
            var token = new JwtSecurityToken(
                issuer: "DojankwonAPI", // ✅ Añadir issuer
                audience: "DojankwonClient", // ✅ Añadir audience
                claims: claims,
                notBefore: DateTime.UtcNow, // ✅ Token válido desde ahora
                expires: DateTime.UtcNow.AddHours(24), // ✅ Extendido a 24 horas
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // ✅ Método adicional para validar tokens
        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var jwtKey = "mi-clave-super-secreta-de-32bytes";
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = "DojankwonAPI",
                    ValidateAudience = true,
                    ValidAudience = "DojankwonClient",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                return tokenHandler.ValidateToken(token, validationParameters, out _);
            }
            catch
            {
                return null;
            }
        }
    }
}