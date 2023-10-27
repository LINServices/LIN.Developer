namespace LIN.Developer.Services.Payment;


public class PayKey : IPayWith
{


    /// <summary>
    /// Guarda el token
    /// </summary>
    private readonly string _key;


    /// <summary>
    /// Acceso HTTP
    /// </summary>
    private readonly IHttpContextAccessor httpContext;


    /// <summary>
    /// Nuevo pago por llave
    /// </summary>
    /// <param name="key">Llave</param>
    public PayKey(string key, IHttpContextAccessor httpContext)
    {
        this._key = key;
        this.httpContext = httpContext;
    }


    /// <summary>
    /// Realiza el cobro
    /// </summary>
    public async Task<ResponseBase> Pay(TransactionDataModel transaction)
    {

        // Conexión a BD
        var (context, conationKey) = Conexión.GetOneConnection();

        // Obtiene la IP
        var myIP = Firewall.HttpIPv4(httpContext);

        // Evalúa el firewall
        var evaluation = await Firewall.EvaluateFirewall(_key, context, myIP);

        // Respuesta
        if (evaluation.Response != Responses.Success)
        {
            context.CloseActions(conationKey);
            return new()
            {
                Response = Responses.Undefined,
                Message = evaluation.Message
            };
        }


        // Modelo del USO
        BillingItemModel uso = new()
        {
            ID = 0,
            Transaction = new()
            {
                Valor = transaction.Valor
            }

        };

        // realiza el cobro
        var result = await Data.ApiKeyUses.GenerateUses(uso, _key, context);

        // Evalúa el cobro
        if (result.Response != Responses.Success)
        {
            return new()
            {
                Response = result.Response,
                Message = "No se pudo efectuar el cobro."
            };
        }

        // Success
        return new()
        {
            Response = Responses.Success
        };
    }


}