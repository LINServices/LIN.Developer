namespace LIN.Developer.Controllers;


[Route("firewallRules")]
public class FirewallRulesController : Controller
{


    /// <summary>
    /// Crea una nueva regla de firewall
    /// </summary>
    /// <param name="modelo">Modelo</param>
    [HttpPost("create")]
    public async Task<HttpCreateResponse> Create([FromBody] FirewallRuleDataModel modelo)
    {

        if (modelo.ProjectID <= 0 || !IP.ValidateIPv4(modelo.IPInicio) || !IP.ValidateIPv4(modelo.IPFinal))
            return new(Responses.InvalidParam);

        // Organización del modelo
        modelo.ID = 0;
        modelo.Status = FirewallRuleStatus.Normal;

        // Respuesta
        var response = await Data.FirewallRules.Create(modelo);

        return response;

    }



    /// <summary>
    /// Obtiene los accesos rechazados de un proyecto
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    [HttpGet("read/bad")]
    public async Task<HttpReadAllResponse<FirewallBlockLogDataModel>> ReadAll([FromHeader] int id, [FromHeader] string token)
    {

        if (id <= 0)
            return new(Responses.InvalidParam);

        var response = await Data.BlockedIPs.ReadAll(id);

        return response;

    }



    /// <summary>
    /// Obtiene los accesos rechazados de un proyecto
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    [HttpDelete("delete/bad")]
    public async Task<HttpResponseBase> DeleteBad([FromHeader] int id, [FromHeader] string token)
    {

        if (id <= 0)
            return new(Responses.InvalidParam);

        var response = await Data.BlockedIPs.Delete(id);

        return response;

    }



    /// <summary>
    /// Obtiene la lista de IP asociados a un proyecto
    /// </summary>
    /// <param name="id">ID del proyecto</param>
    [HttpGet("read/all")]
    public async Task<HttpReadAllResponse<FirewallRuleDataModel>> ReadAll([FromHeader] int id)
    {

        if (id <= 0)
            return new(Responses.InvalidParam);

        var response = await Data.FirewallRules.ReadAll(id);

        return response;

    }



    /// <summary>
    /// Elimina una regla
    /// </summary>
    /// <param name="id">ID de la regla</param>
    /// <param name="token">Token de acceso</param>
    [HttpDelete("delete")]
    public async Task<HttpResponseBase> Delete([FromHeader] int id, [FromHeader] string token)
    {

        if (id <= 0 || token.Length <= 0)
            return new(Responses.InvalidParam);

        // Respuesta
        var response = await Data.FirewallRules.Delete(id);

        return response;

    }



}