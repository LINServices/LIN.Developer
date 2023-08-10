namespace LIN.Developer.Controllers;


[Route("apiKey")]
public class ApiKeyController : Controller
{


    /// <summary>
    /// Crea una llave
    /// </summary>
    /// <param name="modelo">Modelo de la llave</param>
    [HttpPost("create")]
    public async Task<HttpCreateResponse> Create([FromBody] ApiKeyDataModel modelo)
    {

        if (modelo.ProjectID <= 0)
            return new(Responses.InvalidParam);

        // Organización del modelo
        modelo.ID = 0;
        modelo.Status = ApiKeyStatus.Actived;
        modelo.Key = KeyGen.Generate(35, "pk.");

        // Respuesta
        var response = await Data.ApiKeys.Create(modelo);

        return response;

    }



    /// <summary>
    /// Obtiene la lista de llaves asociados a un proyecto
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    [HttpGet("read/all")]
    public async Task<HttpReadAllResponse<ApiKeyDataModel>> ReadAll([FromHeader] int id)
    {

        if (id <= 0)
            return new(Responses.InvalidParam);

        var response = await Data.ApiKeys.ReadAll(id);

        return response;

    }



    /// <summary>
    /// Elimina una llave
    /// </summary>
    /// <param name="key">ID de la llave</param>
    [HttpDelete("delete")]
    public async Task<HttpResponseBase> Delete([FromHeader] int key)
    {
        if (key <= 0)
            return new(Responses.InvalidParam);

        var response = await Data.ApiKeys.UpdateState(key, ApiKeyStatus.Deleted);
        return response;
    }



    /// <summary>
    /// Cambia el estado
    /// </summary>
    /// <param name="key">ID de la llave</param>
    /// <param name="estado">Nuevo estado</param>
    [HttpPatch("update/state")]
    public async Task<HttpResponseBase> ChangeState([FromHeader] int key, [FromHeader] ApiKeyStatus estado)
    {
        if (key <= 0)
            return new(Responses.InvalidParam);

        var response = await Data.ApiKeys.UpdateState(key, estado);
        return response;
    }



}
