using LIN.Developer.Data.Sql;

namespace LIN.Developer.Controllers;


[Route("profile")]
public class ProfilesController : ControllerBase
{


    /// <summary>
    /// Crea un nuevo perfil
    /// </summary>
    /// <param name="modelo">Modelo</param>
    [HttpPost("create")]
    public async Task<HttpCreateResponse> Generate([FromBody] ProfileDataModel modelo, [FromHeader] string token)
    {

        // Validación de parámetros.
        if (modelo == null)
            return new()
            {
                Response = Responses.InvalidParam,
                Message = "Parámetros inválidos."
            };

        // Validación del Email.
        if (modelo.Email == null || !Mail.Validar(modelo.Email))
            return new CreateResponse(Responses.InvalidParam)
            {
                Message = $"El email '{modelo.Email ?? ""}' es invalido."
            };

        // Respuesta de LIN Auth.
        var tokenResponse = await Access.Auth.Controllers.Authentication.Login(token);

        // Validación de la respuesta de LIN Auth.
        if (tokenResponse.Response != Responses.Success)
            return new(Responses.Unauthorized);

        // Créditos.
        decimal defaultCredits = 500m;

        // Organización del modelo.
        modelo.ID = 0;
        modelo.Credits = 0;
        modelo.Estado = ProfileStatus.Waiting;
        modelo.AccountID = tokenResponse.Model.ID;
        modelo.Discount = 0;
        modelo.Transactions = new();

        // Crear el perfil.
        var response = await Profiles.Create(modelo);

        // Respuesta.
        switch (response.Response)
        {
            case Responses.ExistAccount:
                return new CreateResponse()
                {
                    Response = Responses.ExistAccount,
                    Message = "Ya existe el perfil"
                };
            case Responses.Success:
                break;

            default:
                return new CreateResponse()
                {
                    Response = response.Response,
                    Message = "Hubo un error al crear el perfil."
                };

        }

        // Código OTP.
        OTPDataModel otp = new()
        {
            Estado = OTPStatus.actived,
            OTP = KeyGen.GenerateOTP(5),
            Profile = new()
            {
                ID = response.LastID
            },
            Vencimiento = DateTime.Now.AddMinutes(10)
        };

        // Respuesta.
        var otpResponse = await OTP.Create(otp);

        // Si no se guardo el código.
        if (otpResponse.Response != Responses.Success)
        {
            // Informar sobre el error.
        }
        else
        {
            EmailWorker.SendCode(modelo.Email, otp.OTP);
        }

        // Establecer los créditos de regalo.
        var credits = new TransactionDataModel
        {
            ID = 0,
            Description = "Regalo de LIN",
            Valor = defaultCredits,
            Tipo = TransactionTypes.Gift,
            Profile = new()
            {
                ID = response.LastID
            },
            Fecha = DateTime.Now
        };

        // Generar la transacción.
        _ = Transactions.Generate(credits);


        return response;

    }




    /// <summary>
    /// Crea un nuevo perfil
    /// </summary>
    /// <param name="modelo">Modelo</param>
    [HttpPost("testmail")]
    public async Task<dynamic> Generate([FromQuery] string mail)
    {

        await EmailWorker.SendCode(mail, "727722");

        return "A";

    }











    /// <summary>
    /// Obtiene un perfil
    /// </summary>
    /// <param name="id">ID de la cuenta</param>
    [HttpGet("read")]
    public async Task<HttpReadOneResponse<ProfileDataModel>> Read([FromHeader] int id)
    {

        // Obtiene el usuario
        var response = await Profiles.ReadByUser(id);

        if (response.Response != Responses.Success)
            return response;

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
        var response = await Profiles.FindByUser(id);

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
        var response = await Profiles.HasProfile(id);

        // Retorna el resultado
        return response;

    }


}