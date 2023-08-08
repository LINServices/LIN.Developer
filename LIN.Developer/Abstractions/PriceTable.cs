namespace LIN.Developer.Abstractions;


public class PriceTable
{


    /// <summary>
    /// Precio actual de LIN IA Vision
    /// </summary>
    internal static decimal VisionIA { get; private set; } = 10m;


    /// <summary>
    /// Precio actual de LIN IA Names
    /// </summary>
    internal static decimal NamesIA { get; private set; } = 2m;


    /// <summary>
    /// Precio actual de LIN IA Lang
    /// </summary>
    internal static decimal LangIA { get; private set; } = 1;


    /// <summary>
    /// Precio actual de LIN IA Sentiment
    /// </summary>
    internal static decimal SentimentIA { get; private set; } = 7m;


    /// <summary>
    /// Precio actual de LIN Maps
    /// </summary>
    internal static decimal Maps { get; private set; } = 5m;


}