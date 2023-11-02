namespace LIN.Developer.Services;


public class Http
{


    /// <summary>
    /// Obtiene la IP desde un IHttpContext
    /// </summary>
    /// <param name="http">Contexto</param>
    public static Platforms GetPlatform(IHttpContextAccessor http)
    {

        // Obtener el contexto HTTP actual
        var httpContext = http.HttpContext;

        if (httpContext == null || http.HttpContext == null)
            return Platforms.Undefined;


        var userAgent = http.HttpContext.Request.Headers["User-Agent"].ToString().ToLower();

        var pl = http.HttpContext.Request.Headers["sec-ch-ua-platform"].ToString().ToLower();
        
        if (userAgent.Contains("windows"))
            return Platforms.Windows;

        else if (userAgent.Contains("mac"))
            return Platforms.Mac;

        else if (userAgent.Contains("linux"))
            return Platforms.Linux;

        else if (userAgent.Contains("android"))
            return Platforms.Android;





        return Platforms.Undefined;
    }





}
