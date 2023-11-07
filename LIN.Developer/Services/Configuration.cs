namespace LIN.Developer.Services;


public class Configuration
{

    /// <summary>
    /// Configuración.
    /// </summary>
    private static IConfigurationRoot? Config;



    /// <summary>
    /// Esta cargada.
    /// </summary>
    private static readonly bool IsStart = false;



    /// <summary>
    /// Obtiene una configuración.
    /// </summary>
    /// <param name="route">Ruta de acceso.</param>
    public static string GetConfiguration(string route)
    {

        if (!IsStart || Config == null)
        {
            Config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        }

        var b = Config[route] ?? "";
        return b;

    }


}