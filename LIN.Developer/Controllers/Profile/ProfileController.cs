using LIN.Types.Developer.Enumerations;

namespace LIN.Developer.Controllers.Profile;


[Route("profile/security")]
public class ProfileController : ControllerBase
{


    /// <summary>
    /// Valida el codigo OTP para activar el perfil
    /// </summary>
    /// <param name="id">ID del pefil</param>
    /// <param name="otp">Codigo OTP</param>
    [HttpPost("validate/otp")]
    public async Task<ResponseBase> ValidateOTP([FromHeader] int id, [FromHeader] string otp)
    {

        // Valida los parametros
        if (id <= 0 || otp.Length <= 0)
            return new(Responses.InvalidParam);


        // Conexion
        var (context, connectionKey) = Conexión.GetOneConnection();

        // Cambia el estado del codigo OTP
        var otpRes = await Data.OTP.UpdateState(id, otp, context);

        // Valida la respuesta
        if (otpRes.Response != Responses.Success)
        {
            context.CloseActions(connectionKey);
            return new(Responses.Unauthorized);
        }


        // Cambia el estado del pefil desarrollador
        var perfilRes = await Data.Profiles.UpdateState(id, ProfileStatus.Normal);


        // Valida la respuesta
        if (perfilRes.Response != Responses.Success)
        {
            context.CloseActions(connectionKey);
            return new(Responses.NotExistProfile);
        }


        // Perfil desarrollador
        var profile = await Data.Profiles.ReadBy(id);


        // Evaluacion de la respuesta
        if (profile.Response != Responses.Success)
        {
            context.CloseActions(connectionKey);
            return new(Responses.NotExistProfile);
        }


        // Evaluacion de Promocion
        if (Services.Promocion.Promocion.IsPromotionMail(profile.Model.Email ?? ""))
        {

            // Modelo
            var promocion = new TransactionDataModel()
            {
                ID = 0,
                Description = "Bonus",
                Valor = 300m,
                Tipo = TransactionTypes.Bonus,
                ProfileID = id,
                Fecha = DateTime.Now
            };

            _ = Data.Transactions.Generate(promocion, context,  true);

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

        // Valida los parametros
        if (id <= 0 || !LIN.Modules.Mail.Validar(newEmail))
            return new(Responses.InvalidParam);


        // Obtiene una conexion
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
                Message = "No se pudo actualizar el correo electronico"
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


        // Evalua
        if (saveOTP.Response != Responses.Success)
        {
            context.CloseActions(connectionKey);
            return new(Responses.Undefined)
            {
                Message = "No se pudo crear el codigo OTP."
            };
        }


        // Cierra la conexion BD
        context.CloseActions(connectionKey);


        // Envia el codigo al nuevo mail
        EmailWorker.SendCode(newEmail, otpModel.OTP);


        // Correcto
        return new(Responses.Success);

    }



}