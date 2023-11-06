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

        if (modelo == null)
            return new(Responses.InvalidParam);

        // Respuesta de LIN Auth
        var tokenResponse = await Access.Auth.Controllers.Authentication.Login(token);

        // Validación de la respuesta
        if (tokenResponse.Response != Responses.Success)
            return new(Responses.Unauthorized);

        // General
        decimal defaultCreditos = 500m;

        // Organización del modelo
        modelo.ID = 0;
        modelo.Credits = 0;
        modelo.Estado = ProfileStatus.Waiting;
        modelo.AccountID = tokenResponse.Model.ID;
        modelo.Discount = 0;


        if (modelo.Email == null || !LIN.Modules.Mail.Validar(modelo.Email))
            return new CreateResponse(Responses.InvalidParam);


        var response = await Profiles.Create(modelo);


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
                    Message = "Hubo un error"
                };

        }




        var otpCode = KeyGen.GenerateOTP(5);

        var otpModel = new OTPDataModel
        {
            Estado = OTPStatus.actived,
            OTP = otpCode,
            Profile = new()
            {
                ID = response.LastID
            },
            Vencimiento = DateTime.Now.AddMinutes(10),
        };

        var otpResponse = await OTP.Create(otpModel);

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
            Profile = new()
            {
                ID = response.LastID
            },
            Fecha = DateTime.Now
        };

        _ = Transactions.Generate(creditos);



        return response;

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