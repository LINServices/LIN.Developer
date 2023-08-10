namespace LIN.Developer.Controllers;


[Route("apiKey")]
public class ApiKeyController : Controller
{


    /// <summary>
    /// Crea una llave de acceso
    /// </summary>
    /// <param name="modelo">Modelo</param>
    /// <param name="token">Token de acceso</param>
    [HttpPost("create")]
    public async Task<HttpCreateResponse> Create([FromBody] ApiKeyDataModel modelo, [FromHeader] string token)
    {


        // Verifica el acceso
        var haveAccess = await HaveAccess(modelo.ProjectID, token);

        // Si no hay acceso
        if (haveAccess.Response != Responses.Success)
        {
            return new CreateResponse()
            {
                Message = haveAccess.Message,
                Response = haveAccess.Response
            };
        }

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
    public async Task<HttpReadAllResponse<ApiKeyDataModel>> ReadAll([FromHeader] int id, [FromHeader] string token)
    {

        // Verifica el acceso
        var haveAccess = await HaveAccess(id, token);

        // Si no hay acceso
        if (haveAccess.Response != Responses.Success)
        {
            return new ReadAllResponse<ApiKeyDataModel>()
            {
                Message = haveAccess.Message,
                Response = haveAccess.Response
            };
        }

        // Consulta las llaves
        var response = await Data.ApiKeys.ReadAll(id);

        return response;

    }



    /// <summary>
    /// Elimina una llave
    /// </summary>
    /// <param name="key">ID de la llave</param>
    [HttpDelete("delete")]
    public async Task<HttpResponseBase> Delete([FromHeader] int key, [FromHeader] string token)
    {

        if (key <= 0)
            return new(Responses.InvalidParam);

        var keyModel = await Data.ApiKeys.ReadBy(key);

        if (keyModel.Response != Responses.Success)
        {
            return new ResponseBase()
            {
                Message = "No se encontró la api key",
                Response = Responses.NotRows
            };
        }

        // Verifica el acceso
        var haveAccess = await HaveAccess(keyModel.Model.ProjectID, token);

        // Si no hay acceso
        if (haveAccess.Response != Responses.Success)
        {
            return new ReadAllResponse<ApiKeyDataModel>()
            {
                Message = haveAccess.Message,
                Response = haveAccess.Response
            };
        }


        var response = await Data.ApiKeys.UpdateState(key, ApiKeyStatus.Deleted);
        return response;
    }



    /// <summary>
    /// Cambia el estado
    /// </summary>
    /// <param name="key">ID de la llave</param>
    /// <param name="estado">Nuevo estado</param>
    [HttpPatch("update/state")]
    public async Task<HttpResponseBase> ChangeState([FromHeader] int key, [FromHeader] ApiKeyStatus estado, [FromHeader] string token)
    {

        if (key <= 0)
            return new(Responses.InvalidParam);

        var keyModel = await Data.ApiKeys.ReadBy(key);

        if (keyModel.Response != Responses.Success)
        {
            return new ResponseBase()
            {
                Message = "No se encontró la api key",
                Response = Responses.NotRows
            };
        }

        // Verifica el acceso
        var haveAccess = await HaveAccess(keyModel.Model.ProjectID, token);

        // Si no hay acceso
        if (haveAccess.Response != Responses.Success)
        {
            return new ReadAllResponse<ApiKeyDataModel>()
            {
                Message = haveAccess.Message,
                Response = haveAccess.Response
            };
        }

        var response = await Data.ApiKeys.UpdateState(key, estado);
        return response;
    }








    private static async Task<ResponseBase> HaveAccess(int project, string token)
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
