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
    public async Task<ResponseBase> Pay(TransactionDataModel transaccion)
    {

        // Conexion a BD
        var (context, conexionKey) = Conexión.GetOneConnection();

        // Obtiene la IP
        var myIP = Firewall.HttpIPv4(httpContext);

        // Evalua el firewall
        var evaluacion = await Firewall.EvaluateFirewall(_key, context, myIP);

        // Respuesta
        if (evaluacion.Response != Responses.Success)
        {
            context.CloseActions(conexionKey);
            return new()
            {
                Response = Responses.Undefined,
                Message = evaluacion.Message
            };
        }


        // Modelo del USO
        ApiKeyUsesDataModel uso = new()
        {
            ID = 0,
            Valor = transaccion.Valor
        };

        // realiza el cobro
        var result = await Data.ApiKeyUses.GenerateUses(uso, _key, context);

        // Evalua el cobro
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