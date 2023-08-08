using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LIN.Developer.Services;


public class Jwt
{


    /// <summary>
    /// Genera un token JSON
    /// </summary>
    /// <param name="user">Modelo de usuario</param>
    internal static string Generate(ProfileDataModel user)
    {

        // Clave del JWT
        var clave = Configuration.GetConfiguration("jwt:key");

        // Configuracion
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clave));

        // Credenciales
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

        // Reclamaciones
        var claims = new[]
        {
            new Claim(ClaimTypes.PrimarySid, user.ID.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString())
        };

        // Expiracion del token
        var expiracion = DateTime.Now.AddHours(5);

        // Token
        var token = new JwtSecurityToken(null, null, claims, null, expiracion, credentials);

        // Genera el token
        return new JwtSecurityTokenHandler().WriteToken(token);
    }



    /// <summary>
    /// Valida un token JSON
    /// </summary>
    /// <param name="token">Token a validar</param>
    internal static (bool isValid, int account, int profile) Validate(string token)
    {
        try
        {

            // Clave del JWT
            var clave = Configuration.GetConfiguration("jwt:key");

            // Configurar la clave secreta
            var key = Encoding.ASCII.GetBytes(clave);

            // Validar el token
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
            };

            try
            {

                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;


                // Si el token es válido, puedes acceder a los claims (datos) del usuario
                var user = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

                // 
                _ = int.TryParse(jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.PrimarySid)?.Value, out int profileID);


                _ = int.TryParse(jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value, out int accountID);


                // Devuelve una respuesta exitosa
                return (true, accountID, profileID);

            }
            catch (SecurityTokenException)
            {

            }


        }
        catch { }

        return (false, 0, 0);




    }


}

