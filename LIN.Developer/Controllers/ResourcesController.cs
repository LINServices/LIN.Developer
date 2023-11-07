namespace LIN.Developer.Controllers;


[Route("resources")]
public class ResourcesController : Controller
{


    /// <summary>
    /// Crea un nuevo recurso. 
    /// </summary>
    /// <param name="modelo">Modelo</param>
    [HttpPost("create")]
    public async Task<HttpCreateResponse> Create([FromBody] ResourceModel modelo, [FromHeader] string token)
    {

        // Validaciones del modelo.
        if (modelo.Name.Length <= 0)
            return new(Responses.InvalidParam)
            {
                Message = "Parámetros inválidos."
            };

        // Validación de token
        var (isValid, _, profile) = Jwt.Validate(token);

        // Si es invalido
        if (!isValid)
            return new CreateResponse()
            {
                Message = "Token invalido",
                Response = Responses.Unauthorized
            };


        // Organización del modelo.
        modelo.ProfileId = profile;
        modelo.Status = ProjectStatus.Normal;
        modelo.Allowed = new List<AccessModel>()
        {
            new()
            {
                IamLevel = IamLevels.Privileged,
                Profile = profile
            }
        };

        // Respuesta
        var response = await Data.Mongo.Resources.Create(modelo);

        return response;

    }



    /// <summary>
    /// Obtiene los recursos asociados a un perfil.
    /// </summary>
    /// <param name="token">Token de acceso.</param>
    [HttpGet("read/all")]
    public async Task<HttpReadAllResponse<ResourceModel>> ReadAll([FromHeader] string token)
    {

        // Información del token.
        var (isValid, _, profile) = Jwt.Validate(token);

        // Si el token es invalido.
        if (!isValid)
            return new(Responses.Unauthorized)
            {
                Message = "Token invalido."
            };

        // Validar parámetros.
        if (profile <= 0 || token.IsNullOrEmpty())
            return new(Responses.InvalidParam);


        var response = await Data.Mongo.Resources.ReadAll(profile);

        return response;

    }



    /// <summary>
    /// Obtiene un recurso.
    /// </summary>
    /// <param name="id">ID del recurso</param>
    /// <param name="token">Token de acceso</param>
    [HttpGet("read")]
    public async Task<HttpReadOneResponse<ResourceModel>> Read([FromHeader] string id, [FromHeader] string token)
    {

        // Validaciones.
        if (id.Trim().Length <= 0 || token.Length <= 0)
            return new(Responses.InvalidParam);

        // Información del token.
        var (isValid, _, profile) = Jwt.Validate(token);

        // Validación del token.
        if (!isValid)
            return new(Responses.Unauthorized)
            {
                Message = "Token invalido."
            };

        // Validar acceso IAM.
        var iam = await Services.Iam.Resource.Validate(profile, id);

        // Validación de respuesta Iam.
        if (iam.Response != Responses.Success)
            return new(Responses.NotRows)
            {
                Message = $"No se encontró el recurso '{id}'."
            };

        // No autorizado por Iam.
        if (iam.Model == IamLevels.NotAccess)
            return new(Responses.NotRows)
            {
                Message = $"No tienes acceso a este recurso."
            };

        // Obtiene el recurso.
        var response = await Data.Mongo.Resources.Read(id);

        return response;

    }



    /// <summary>
    /// Elimina un recurso.
    /// </summary>
    /// <param name="id">ID del recurso</param>
    /// <param name="token">Token de acceso</param>
    [HttpDelete("delete")]
    public async Task<HttpReadOneResponse<bool>> Delete([FromHeader] int id, [FromHeader] string token)
    {

        // Valida los permisos del perfil en un proyecto
        var access = await HaveAccess(id, token);

        // Validación
        if (access.Response != Responses.Success)
        {
            return new ReadOneResponse<bool>()
            {
                Message = access.Message,
                Response = access.Response
            };
        }

        // Respuesta
        var response = await Data.Mongo.Resources.Delete(id);

        return response;

    }



    /// <summary>
    /// Valida si un usuario tiene acceso a un proyecto
    /// </summary>
    /// <param name="project">ID del proyecto</param>
    /// <param name="token">Token de acceso</param>
    [Obsolete("Este método debe ser eliminado en favor de LIN IAM")]
    public static async Task<ResponseBase> HaveAccess(int project, string token)
    {

        // Validación del JWT
        var (isValid, _, profile) = Jwt.Validate(token);

        if (!isValid)
            return new ResponseBase()
            {
                Message = "Token invalido",
                Response = Responses.Unauthorized
            };

        // Validación del parámetro
        if (project <= 0)
            return new ResponseBase()
            {
                Message = "ID del proyecto es invalido",
                Response = Responses.InvalidParam
            };

        // Tiene acceso al proyecto
        var have = await Data.Mongo.Resources.HaveAuthorization(project, profile);

        // Si no tubo acceso
        if (have.Response != Responses.Success)
            return new ResponseBase()
            {
                Message = "No tienes acceso a este proyecto",
                Response = Responses.Unauthorized
            };


        return new ResponseBase(Responses.Success);

    }



}