namespace LIN.Developer.Areas;


[Route("authentication")]
public class AuthenticationController : ControllerBase
{


    /// <summary>
    /// Iniciar sesión con credenciales.
    /// </summary>
    /// <param name="user">Usuario.</param>
    /// <param name="password">Contraseña.</param>
    [HttpGet("login")]
    public async Task<HttpReadOneResponse<AuthModel<ProfileDataModel>>> Login([FromQuery] string user, [FromQuery] string password)
    {

        // Validación de información.
        if (!user.Trim().Any() || !password.Trim().Any())
            return new ReadOneResponse<AuthModel<ProfileDataModel>>()
            {
                Message = "Parámetros inválidos.",
                Response = Responses.InvalidParam
            };

        // Respuesta de LIN Identity.
        var authResponse = await Access.Auth.Controllers.Authentication.Login(user, password);

        // Error al iniciar.
        if (authResponse.Response != Responses.Success)
            return new(authResponse.Response);

        // Obtiene el perfil.
        var profile = await Data.Profiles.ReadByUser(authResponse.Model.ID);

        // Respuesta final.
        var httpResponse = new ReadOneResponse<AuthModel<ProfileDataModel>>()
        {
            Response = Responses.Success,
            Message = "Success"
        };

        // Si existe el perfil.
        if (profile.Response == Responses.Success)
        {
            // Genera el token
            var token = Jwt.Generate(profile.Model);

            // Establecer el token
            httpResponse.Token = token;
            httpResponse.Model.Profile = profile.Model;
        }

        // Token de identity.
        httpResponse.Model.Account = authResponse.Model;
        httpResponse.Model.TokenCollection = new()
        {
            {"identity", authResponse.Token}
        };

        return httpResponse;

    }



    /// <summary>
    /// Obtener sesión con token.
    /// </summary>
    /// <param name="token">Token</param>
    [HttpGet("login/token")]
    public async Task<HttpReadOneResponse<AuthModel<ProfileDataModel>>> LoginToken([FromQuery] string token)
    {

        // Autenticación en Identity.
        var response = await Access.Auth.Controllers.Authentication.Login(token);

        // Si hubo un error.
        if (response.Response != Responses.Success)
            return new(response.Response)
            {
                Message = "Error al autenticar en LIN Identity."
            };

        // Error de integridad con la cuenta.
        if (response.Model.Estado != AccountStatus.Normal)
            return new(Responses.NotExistAccount)
            {
                Message = "No existe esta cuenta."
            };


        // Obtiene el perfil.
        var profile = await Data.Profiles.ReadByUser(response.Model.ID);


        var httpResponse = new ReadOneResponse<AuthModel<ProfileDataModel>>()
        {
            Response = Responses.Success,
            Message = "Success",
        };

        if (profile.Response == Responses.Success)
        {
            // Genera el token
            var tokenAcceso = Jwt.Generate(profile.Model);

            httpResponse.Token = tokenAcceso;
            httpResponse.Model.Profile = profile.Model;
        }

        httpResponse.Model.Account = response.Model;
        httpResponse.Model.TokenCollection = new()
        {
            {"identity", response.Token}
        };

        return httpResponse;

    }


}