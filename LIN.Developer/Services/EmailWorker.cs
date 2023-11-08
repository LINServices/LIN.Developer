namespace LIN.Developer.Services;


public class EmailWorker
{


    /// <summary>
    /// Email de salida
    /// </summary>
    private static string Password { get; set; } = string.Empty;


    /// <summary>
    /// Inicia el servicio
    /// </summary>
    public static void StarService()
    {
        Password = Configuration.GetConfiguration("resend:key");
    }




    /// <summary>
    /// Enviar un correo
    /// </summary>
    /// <param name="to">Destinatario</param>
    public static async Task<bool> SendCode(string to, string code)
    {

        // Obtiene la plantilla
        var body = File.ReadAllText("wwwroot/Plantillas/Code.html");

        // Remplaza.
        body = body.Replace("@@CODE", code);

        // Envía el email.
        return await SendMail(to, "Verificación de email", body);

    }



    /// <summary>
    /// Enviar un correo
    /// </summary>
    /// <param name="to">Destinatario</param>
    /// <param name="asunto">Asunto</param>
    /// <param name="body">Cuerpo del correo</param>
    public static async Task<bool> SendMail(string to, string asunto, string body)
    {
        try
        {

            // Cliente HTTP.
            var client = new HttpClient()
            {
                BaseAddress = new Uri("https://api.resend.com/emails"),
                Timeout = TimeSpan.FromSeconds(10)
            };

            // Autorización.
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Password);

            // Solicitud.
            var request = new
            {
                from = "security@linapps.co",
                to = new[]
                {
                    to
                },
                subject = asunto,
                html = body
            };

            // Serializa a JSON.
            string json = System.Text.Json.JsonSerializer.Serialize(request);

            // Armar el contenido.
            StringContent content = new (json, Encoding.UTF8, "application/json");

            // Enviar.
            var response = await client.PostAsync("", content);

            // Respuesta.
            return response.IsSuccessStatusCode;

        }
        catch
        {
        }
        return false;
    }



}
