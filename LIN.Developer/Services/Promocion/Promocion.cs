namespace LIN.Developer.Services.Promocion;

public class Promocion
{


    public static bool IsPromotionMail(string promotionMail)
    {
        promotionMail = promotionMail.Trim().ToLower();
        string[] mails = { "@misena.edu.co", "@soy.sena.edu.co", "@envigado.edu.co" };

        foreach(string mail in mails)
        {
            if (promotionMail.EndsWith(mail))
                return true;
            
        }
        return false;

    }
}