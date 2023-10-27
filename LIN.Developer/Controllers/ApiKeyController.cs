using LIN.Developer.MongoDBModels;

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
    public async Task<HttpCreateResponse> Create([FromBody] ApiKeyDataModel modelo, [FromHeader] int Id, [FromHeader] string token)
    {

        // Verifica el acceso
        var haveAccess = await ProjectsController.HaveAccess(Id, token);

        // Si no hay acceso
        if (haveAccess.Response != Responses.Success)
        {
            return new CreateResponse()
            {
                Message = haveAccess.Message,
                Response = haveAccess.Response
            };
        }

        var keyModel = new KeyModel()
        {
            Key = KeyGen.Generate(35, "pk."),
            Status = ApiKeyStatus.Actived,
            Nombre = modelo.Nombre
        };


        // Respuesta
        var response = await Data.ApiKeys.Create(keyModel);

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
        var haveAccess = await ProjectsController.HaveAccess(id, token);

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
        //var haveAccess = await ProjectsController.HaveAccess(keyModel.Model.Project.ID, token);

        //// Si no hay acceso
        //if (haveAccess.Response != Responses.Success)
        //{
        //    return new ReadAllResponse<ApiKeyDataModel>()
        //    {
        //        Message = haveAccess.Message,
        //        Response = haveAccess.Response
        //    };
        //}


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
      //  var haveAccess = await ProjectsController.HaveAccess(keyModel.Model.Project.ID, token);

        //// Si no hay acceso
        //if (haveAccess.Response != Responses.Success)
        //{
        //    return new ReadAllResponse<ApiKeyDataModel>()
        //    {
        //        Message = haveAccess.Message,
        //        Response = haveAccess.Response
        //    };
        //}

        var response = await Data.ApiKeys.UpdateState(key, estado);
        return response;
    }


}
