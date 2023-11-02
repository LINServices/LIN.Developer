namespace LIN.Developer.Controllers;


[Route("/")]
public class APIVersion : Controller
{



    /// <summary>
    /// Obtiene el estado del servidor
    /// </summary>
    [HttpGet("status")]
    public dynamic Status()
    {
        return StatusCode(200, new
        {
            Status = "Open"
        });
    }




    [HttpGet]
    public dynamic Index()
    {
        return Ok("Abierto");
    }



    /// <summary>
    /// Obtiene la version de LIN Actual
    /// </summary>
    [HttpGet("Version")]
    public dynamic Version()
    {

        // Obtener el ensamblado actual (el ensamblado de la aplicación)
        Assembly? assembly = Assembly.GetEntryAssembly();

        // Obtener los metadatos del ensamblado
        AssemblyName? assemblyName = assembly?.GetName();
        string name = assemblyName?.Name ?? "";
        string version = assemblyName?.Version?.ToString() ?? "";
        string mode = "undefined";

#if DEBUG
        mode = "debug";
#elif RELEASE
        mode = "release";
#elif AZURE
        mode = "Azure";
#elif SOMEE
        mode = "Somee";
#endif
        // Retorna el resultado
        return new
        {
            Mode = mode,
            Name = name,
            Version = version
        };

    }







}