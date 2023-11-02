namespace LIN.Developer.Controllers.Profile;


[Route("profile/security")]
public class ProfileController : ControllerBase
{


    /// <summary>
    /// Valida el código OTP para activar el perfil
    /// </summary>
    /// <param name="id">ID del perfil</param>
    /// <param name="otp">Código OTP</param>
    [HttpPost("validate/otp")]
    public async Task<ResponseBase> ValidateOTP([FromHeader] int id, [FromHeader] string otp)
    {

        // Valida los parámetros
        if (id <= 0 || otp.Length <= 0)
            return new(Responses.InvalidParam);


        // Conexión
        var (context, connectionKey) = Conexión.GetOneConnection();

        // Cambia el estado del código OTP
        var otpRes = await Data.OTP.UpdateState(id, otp, context);

        // Valida la respuesta
        if (otpRes.Response != Responses.Success)
        {
            context.CloseActions(connectionKey);
            return new(Responses.Unauthorized);
        }


        // Cambia el estado del perfil desarrollador
        var perfilRes = await Data.Profiles.UpdateState(id, ProfileStatus.Normal);


        // Valida la respuesta
        if (perfilRes.Response != Responses.Success)
        {
            context.CloseActions(connectionKey);
            return new(Responses.NotExistProfile);
        }


        // Perfil desarrollador
        var profile = await Data.Profiles.ReadBy(id);


        // Evaluación de la respuesta
        if (profile.Response != Responses.Success)
        {
            context.CloseActions(connectionKey);
            return new(Responses.NotExistProfile);
        }


        // Evaluación de Promoción
        if (Services.Promocion.Promocion.IsPromotionMail(profile.Model.Email ?? ""))
        {

            // Modelo
            var promotion = new TransactionDataModel()
            {
                ID = 0,
                Description = "Bonus",
                Valor = 300m,
                Tipo = TransactionTypes.Bonus,
                ProfileID = id,
                Fecha = DateTime.Now
            };

            _ = Data.Transactions.Generate(promotion, context,  true);

        }


        // Correcto
        return new(Responses.Success);

    }



    /// <summary>
    /// Reemplaza el correo
    /// </summary>
    /// <param name="id">ID del perfil</param>
    /// <param name="newEmail">Nuevo EMAIL</param>
    [HttpPost("onCreating/replace/mail")]
    public async Task<ResponseBase> ReplaceMail([FromHeader] int id, [FromQuery] string newEmail)
    {

        // Valida los parámetros
        if (id <= 0 || !LIN.Modules.Mail.Validar(newEmail))
            return new(Responses.InvalidParam);


        // Obtiene una conexión
        var (context, connectionKey) = Conexión.GetOneConnection();

        // Obtiene la data
        var data = await Data.Profiles.ReadBy(id, context);

        // Valida
        if (data.Response != Responses.Success)
        {
            context.CloseActions(connectionKey);
            return new(Responses.Undefined);
        }


        // Si el perfil ya esta creado
        if (data.Model.Estado != ProfileStatus.Waiting)
        {
            context.CloseActions(connectionKey);
            return new(Responses.Unauthorized);
        }


        // Actualiza el email
        var updateRes = await Data.Profiles.UpdateMail(data.Model.ID, newEmail, context);


        // Respuesta
        if (updateRes.Response != Responses.Success)
        {
            context.CloseActions(connectionKey);
            return new(Responses.Undefined)
            {
                Message = "No se pudo actualizar el correo electrónico"
            };

        }


        // Nuevo OTP
        var otpModel = new OTPDataModel
        {
            ID = 0,
            OTP = KeyGen.GenerateOTP(5),
            Vencimiento = DateTime.Now.AddMinutes(10),
            ProfileID = data.Model.ID,
            Estado = OTPStatus.actived
        };


        // Guarda el OTP
        var saveOTP = await Data.OTP.Create(otpModel, context);


        // Evalúa
        if (saveOTP.Response != Responses.Success)
        {
            context.CloseActions(connectionKey);
            return new(Responses.Undefined)
            {
                Message = "No se pudo crear el código OTP."
            };
        }


        // Cierra la conexión BD
        context.CloseActions(connectionKey);


        // Envía el código al nuevo mail
        EmailWorker.SendCode(newEmail, otpModel.OTP);


        // Correcto
        return new(Responses.Success);

    }



}