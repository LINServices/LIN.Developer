namespace LIN.Developer.Controllers;


[Route("profile")]
public class ProfilesController : ControllerBase
{


    /// <summary>
    /// Crea un nuevo perfil
    /// </summary>
    /// <param name="modelo">Modelo</param>
    [HttpPost("create")]
    public async Task<HttpCreateResponse> Generate([FromBody] ProfileDataModel modelo)
    {

        // General
        decimal defaultCreditos = 500m;

        // Organización del modelo
        modelo.ID = 0;
        modelo.Credito = 0;
        modelo.Estado = ProfileStatus.Waiting;

        if (modelo.Email == null || !LIN.Modules.Mail.Validar(modelo.Email))
            return new CreateResponse(Responses.InvalidParam);



        var response = await Data.Profiles.Create(modelo);

        if (response.Response != Responses.Success)
            return response;



        var otpCode = KeyGen.GenerateOTP(5);

        var otpModel = new OTPDataModel
        {
            Estado = OTPStatus.actived,
            OTP = otpCode,
            ProfileID = response.LastID,
            Vencimiento = DateTime.Now.AddMinutes(10),
        };

        var otpResponse = await Data.OTP.Create(otpModel);

        if (otpResponse.Response == Responses.Success)
        {
            EmailWorker.SendCode(modelo.Email, otpCode);
        }



        // Da los creditos por defecto
        var creditos = new TransactionDataModel()
        {
            ID = 0,
            Description = "LIN Gift",
            Valor = defaultCreditos,
            Tipo = TransactionTypes.Gift,
            ProfileID = response.LastID,
            Fecha = DateTime.Now
        };

        _ = Data.Transactions.Generate(creditos);



        return response;

    }






    /// <summary>
    /// Iniciar sesión
    /// </summary>
    /// <param name="user">Usuario</param>
    /// <param name="password">Contraseña</param>
    [HttpGet("login")]
    public async Task<HttpReadOneResponse<AuthModel<ProfileDataModel>>> Login([FromQuery] string user, [FromQuery] string password)
    {

        // Comprobación
        if (!user.Any() || !password.Any())
            return new(Responses.InvalidParam);


        // Login en LIN Server
        var response = await Access.Auth.Controllers.Account.Login(user, password);

        if (response.Response != Responses.Success)
            return new(response.Response);

        if (response.Model.Estado != AccountStatus.Normal)
            return new(Responses.NotExistAccount);

        

        var profile = await Data.Profiles.ReadByUser(response.Model.ID);


        var httpResponse = new ReadOneResponse<AuthModel<ProfileDataModel>>()
        {
            Response = Responses.Success,
            Message = "Success",

        };

        if (profile.Response == Responses.Success)
        {
            // Genera el token
            var token = Jwt.Generate(profile.Model);

            httpResponse.Token = token;
            httpResponse.Model.Profile = profile.Model;
        }

        httpResponse.Model.Account = response.Model;
        httpResponse.Model.LINAuthToken = response.Token;




        return httpResponse;

    }
























    /// <summary>
    /// Obtiene un perfil
    /// </summary>
    /// <param name="id">ID de la cuenta</param>
    [HttpGet("read")]
    public async Task<HttpReadOneResponse<ProfileDataModel>> ReadOneByID([FromHeader] int id)
    {

        // Obtiene el usuario
        var response = await Data.Profiles.ReadByUser(id);

        if (response.Response != Responses.Success)
            return response;


        response.Token = Jwt.Generate(response.Model);

        return response;

    }



    /// <summary>
    /// Obtiene un perfil
    /// </summary>
    /// <param name="id">ID del perfil</param>
    [HttpGet("find")]
    public async Task<HttpReadOneResponse<ProfileDataModel>> Find([FromHeader] int id)
    {

        // Obtiene el usuario
        var response = await Data.Profiles.FindByUser(id);

        if (response.Response != Responses.Success)
            return response;

        return response;

    }



    /// <summary>
    /// Comprueba si una cuenta tiene perfil dev
    /// </summary>
    /// <param name="id">ID de la cuenta</param>
    [HttpGet("haveFor")]
    public async Task<HttpReadOneResponse<bool>> HasProfile([FromHeader] int id)
    {

        // Obtiene el usuario
        var response = await Data.Profiles.HasProfile(id);

        // Retorna el resultado
        return response;

    }


}