namespace LIN.Developer.Services;


public class Configuration
{

    private static IConfigurationRoot? Config;

    private static readonly bool IsStart = false;


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
