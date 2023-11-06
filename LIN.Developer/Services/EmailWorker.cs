using System.Net;
using System.Net.Mail;

namespace LIN.Developer.Services;


[Obsolete("Este método debe de der reemplazado por un proveedor de mails valido.")]
public class EmailWorker
{



    /// <summary>
    /// Enviar un correo
    /// </summary>
    /// <param name="to">Destinatario</param>
    public static bool SendCode(string to, string code)
    {

        // Obtiene la plantilla
        var body = File.ReadAllText("wwwroot/Plantillas/Code.html");

        // Remplaza
        body = body.Replace("@@CODE", code);

        // Envia el email
        return SendMail(to, "Verficacion de email", body);

    }



    /// <summary>
    /// Enviar un correo
    /// </summary>
    /// <param name="to">Destinatario</param>
    /// <param name="asunto">Asunto</param>
    /// <param name="body">Cuerpo del correo</param>
    public static bool SendMail(string to, string asunto, string body)
    {
        // Configurar los detalles del correo
        string remitente = "giraldojhon055@hotmail.com";
        string contraseña = "";
        string destinatario = to ?? "";
        string cuerpo = body ?? "";

        // Configurar el cliente SMTP de Hotmail/Outlook.com
        SmtpClient smtpClient = new("smtp-mail.outlook.com", 587)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(remitente, contraseña)
        };

        // Crear el mensaje
        MailMessage correo = new(remitente, destinatario, asunto, cuerpo)
        {
            IsBodyHtml = true
        };

        try
        {
            // Enviar el correo
            smtpClient.Send(correo);
            return true;
        }
        catch
        {
        }
        return false;
    }



}
