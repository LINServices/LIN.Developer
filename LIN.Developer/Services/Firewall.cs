namespace LIN.Developer.Services;


public class Firewall
{


    /// <summary>
    /// Obtiene la IP desde un IHttpContext
    /// </summary>
    /// <param name="http">Contexto HTTP</param>
    public static string HttpIPv4(IHttpContextAccessor http)
    {
        // Obtener el contexto HTTP actual
        var httpContext = http.HttpContext;

        if (httpContext == null)
            return "";


        // Obtener la dirección IP del cliente
        var ipAddress = httpContext.Connection.RemoteIpAddress;

        


        // Verificar si la dirección IP es de IPv4 o IPv6
        if (ipAddress != null)
        {
            if (ipAddress.IsIPv4MappedToIPv6)
                ipAddress = ipAddress.MapToIPv4();

            // ipAddress ahora contiene la dirección IP del cliente
            var ipString = ipAddress.ToString();
            return ipString;
        }

        return "";
    }



    /// <summary>
    /// Evalúa el Firewall
    /// </summary>
    /// <param name="apikey">Llave de acceso a API</param>
    /// <param name="contextConnection">Contexto de conexión a BD</param>
    /// <param name="ip">IP A evaluar</param>
    public static async Task<ResponseBase> EvaluateFirewall(string apiKey, Conexión contextConnection, string ip)
    {

        // Si el cliente es el servidor
        if (ip == "::1")
            return new(Responses.Success);

        // obtener el ID de un proyecto asociado a un Key
        var projectID = await Data.ApiKeys.GetProjectID(apiKey, contextConnection);

        // Si hay algún error
        if (projectID.Response != Responses.Success)
            return new(Responses.DontHavePermissions)
            {
                Message = projectID.Message
            };

        // Recupera el ID
        int id = projectID.Model;

        // Comprueba si el proyecto tiene reglas firewall para una IP
        var has = await Data.Projects.HasFirewallFor(id, ip, contextConnection);

        // Evalúa el Has
        if (has.Response != Responses.Success)
        {

            if (IP.ValidateIPv4(ip))
            {
                // Registra el acceso rechazado
                _ = Data.BlockedIPs.Create(new()
                {
                    IPv4 = ip,
                    ProyectoID = id,
                    Estado = FirewallBlockStatus.Normal
                });

                return new(Responses.FirewallBlocked)
                {
                    Message = $"La IP '{ip}' no esta permitida en este proyecto, revisa la configuración del firewall de tu proyecto."
                };

            }

            return new(Responses.FirewallBlocked)
            {
                Message = $"No se logro validar la IP del cliente."
            };

        }

        return new(Responses.Success);


    }


}