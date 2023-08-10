using LIN.Types.Developer.Enumerations;
using LIN.Types.Developer.Models;

namespace LIN.Developer.Controllers;


[Route("project")]
public class ProjectsController : Controller
{


    /// <summary>
    /// Crea un nuevo proyecto 
    /// </summary>
    /// <param name="modelo">Modelo</param>
    [HttpPost("create")]
    public async Task<HttpCreateResponse> Create([FromBody] ProjectDataModel modelo)
    {

        // Validaciones
        if (modelo.ProfileID <= 0 || modelo.Nombre.Length <= 0)
            return new(Responses.InvalidParam);

        // Organización del modelo
        modelo.ID = 0;
        modelo.Creacion = DateTime.Now;
        modelo.Estado = ProjectStatus.Normal;

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
        var (isValid, _, profile) = Jwt.Validate(token);

        // Token invalido
        if (!isValid)
            return new(Responses.Unauthorized);

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

        var (isValid, _, _) = Jwt.Validate(token);

        if (!isValid)
            return new(Responses.Unauthorized);


        // Respuesta
        var response = await Data.Projects.Delete(id);

        return response;

    }



}