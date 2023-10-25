namespace LIN.Developer.Controllers;


[Route("project")]
public class ProjectsController : Controller
{


    /// <summary>
    /// Crea un nuevo proyecto 
    /// </summary>
    /// <param name="modelo">Modelo</param>
    [HttpPost("create")]
    public async Task<HttpCreateResponse> Create([FromBody] ProjectDataModel modelo, [FromHeader] string token)
    {

        // Validaciones
        if (modelo.Nombre.Length <= 0)
            return new(Responses.InvalidParam);

        // Validación de token
        var (isValid, _, profile) = Jwt.Validate(token);

        // Si es invalido
        if (!isValid)
        {
            return new CreateResponse()
            {
                Message = "Token invalido",
                Response = Responses.Unauthorized
            };
        }

        // Organización del modelo
        modelo.ID = 0;
        modelo.Creacion = DateTime.Now;
        modelo.Estado = ProjectStatus.Normal;
        modelo.Profile = new()
        {
            ID = profile
        };

        // Respuesta
        var response = await Data.Projects.Create(modelo);

        return response;

    }



    /// <summary>
    /// Obtiene los proyectos asociados a un perfil
    /// </summary>
    /// <param name="token">Token de acceso</param>
    [HttpGet("read/all")]
    public async Task<HttpReadAllResponse<ProjectDataModel>> ReadAll([FromHeader] string token)
    {

        var (isValid, _, profile) = Jwt.Validate(token);

        if (!isValid)
            return new(Responses.Unauthorized);

        if (profile <= 0 || token.IsNullOrEmpty())
            return new(Responses.InvalidParam);



        // Token invalido
        if (!isValid)
            return new(Responses.Unauthorized);

        var response = await Data.Projects.ReadAll(profile);

        return response;

    }



    /// <summary>
    /// Obtiene un proyecto
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    /// <param name="token">Token de acceso</param>
    [HttpGet("read")]
    public async Task<HttpReadOneResponse<ProjectDataModel>> Read([FromHeader] int id, [FromHeader] string token)
    {

        // Validaciones
        if (id <= 0 || token.Length <= 0)
            return new(Responses.InvalidParam);

        // Valida el token
        var access = await HaveAccess(id, token);


        // Si es invalido
        if (access.Response != Responses.Success)
        {
            return new ReadOneResponse<ProjectDataModel>
            {
                Message = access.Message,
                Response = access.Response
            };
        }

        // Obtiene el profile
        (_, _, int profile) = Jwt.Validate(token);

        // Obtiene los proyectos
        var response = await Data.Projects.Read(id, profile);

        return response;

    }



    /// <summary>
    /// Elimina un proyecto
    /// </summary>
    /// <param name="id">ID del proyecto</param>
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
        var response = await Data.Projects.Delete(id);

        return response;

    }





    /// <summary>
    /// Valida si un usuario tiene acceso a un proyecto
    /// </summary>
    /// <param name="project">ID del proyecto</param>
    /// <param name="token">Token de acceso</param>
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
        var have = await Data.Projects.HaveAuthorization(project, profile);

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