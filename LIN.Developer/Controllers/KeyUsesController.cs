using LIN.Developer.Data.Sql;

namespace LIN.Developer.Controllers;


[Route("key/uses")]
public class KeyUsesController : Controller
{


    /// <summary>
    /// Genera un uso a una llave
    /// </summary>
    /// <param name="modelo">Modelo del uso</param>
    /// <param name="apiKey">Llave de acceso</param>
    /// <param name="http">Contexto HTTP</param>
    [HttpPost("create")]
    public async Task<HttpCreateResponse> Create([FromBody] BillingItemModel modelo, [FromHeader] string apiKey, [FromServices] IHttpContextAccessor http)
    {


        // Conexión a BD
        var (context, contextKey) = Conexión.GetOneConnection();

        // Obtiene la IP
        var myIP = Firewall.HttpIPv4(http);

        // Evalúa el firewall
        var evaluation = await Firewall.EvaluateFirewall(apiKey, context, myIP);

        // Respuesta
        if (evaluation.Response != Responses.Success)
        {
            context.CloseActions(contextKey);

            return new CreateResponse(evaluation.Response)
            {
                Message = evaluation.Message
            };
        }

        // Organización del modelo
        modelo.ID = 0;
        modelo.Transaction.ID = 0;

        var response = await ApiKeyUses.GenerateUses(modelo, apiKey, context);

        context.CloseActions(contextKey);

        return response;

    }




    /// <summary>
    /// Da creditos a una cuenta
    /// </summary>
    [HttpPost("giveCredicts")]
    [Obsolete("Esta es una funcion de prueba")]
    public bool Give([FromHeader] decimal credito, [FromHeader] int id)
    {

        try
        {
            var (conexion, conexionKey) = Conexión.GetOneConnection();
            var modelo = conexion.DataBase.Profiles.Find(id);

            if (modelo == null)
                return false;

            modelo.Credits = credito;
            conexion.DataBase.SaveChanges();
            return true;

        }
        catch
        {

        }

        return false;

    }




    /// <summary>
    /// Da creditos a una cuenta
    /// </summary>
    [HttpPost("giveCredicts2")]
    [Obsolete("Esta es una funcion de prueba")]
    public async Task<CreateResponse> Give2([FromHeader] decimal credito, [FromHeader] int id)
    {

        try
        {
            var transaccion = new TransactionDataModel
            {
                Description = "Regalo",
                Fecha = DateTime.Now,
                Profile = new() {ID = id },
                Tipo = TransactionTypes.Recharge,
                Valor = credito
            };

            return await Transactions.Generate(transaccion);

        }
        catch
        {

        }

        return new();


    }



}