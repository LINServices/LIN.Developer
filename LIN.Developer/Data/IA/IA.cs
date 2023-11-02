namespace LIN.Developer.Data.IA;
using LIN.Types.Inventory.Enumerations;

public static class IA
{


    /// <summary>
    /// Categoriza el idioma de un string
    /// </summary>
    /// <param name="value">Texto</param>
    public static Languajes Lang(string value)
    {
        try
        {
            // Prediccion
            var prediccion = new LangsIA.ModelInput()
            {
                Col1 = value
            };

            // Resultado
            var predict = LangsIA.Predict(prediccion).PredictedLabel;

            // Manejo
            return predict switch
            {
                0 => Languajes.Spain,
                1 => Languajes.English,
                _ => Languajes.Undefined,
            };
        }
        catch (Exception ex)
        {
            ServerLogger.LogError("Lingua IA: " + ex.Message);
        }

        return Languajes.Undefined;
    }



    /// <summary>
    /// Categoriza el genero de un nombre
    /// </summary>
    /// <param name="value">Nombre</param>
    public static Genders Gender(string value)
    {
        try
        {
            // Prediccion
            var prediccion = new NombreIA.ModelInput()
            {
                Col0 = value
            };

            // Resultado
            var predict = NombreIA.Predict(prediccion).PredictedLabel;

            // Manejo
            return predict switch
            {
                0 => Genders.Female,
                1 => Genders.Male,
                _ => Genders.Undefined,
            };
        }
        catch (Exception ex)
        {
            ServerLogger.LogError("Genderize: " + ex.Message);
        }

        return Genders.Undefined;
    }



    /// <summary>
    /// Categoriza el sentimiento de un texto
    /// </summary>
    /// <param name="value">Texto</param>
    public static Sentiments Sentiment(string value)
    {
        try
        {
            // Prediccion
            var prediccion = new SentimentIA.ModelInput()
            {
                Col1 = value
            };

            // Resultado
            var predict = SentimentIA.Predict(prediccion).PredictedLabel;

            // Manejo
            return predict switch
            {
                0 => Types.Enumerations.Sentiments.Negative,
                1 => Types.Enumerations.Sentiments.Positive,
                4 => Types.Enumerations.Sentiments.Positive,
                _ => Types.Enumerations.Sentiments.Undefined
            };
        }
        catch (Exception ex)
        {
            ServerLogger.LogError("EmoSense: " + ex.Message);
        }

        return Types.Enumerations.Sentiments.Undefined;
    }



    /// <summary>
    /// Categoriza el sentimiento de un texto
    /// </summary>
    /// <param name="value">Texto</param>
    public static Sentiments EmoSenses(string value)
    {
        try
        {
            // Prediccion
            var prediccion = new EmoSense.ModelInput()
            {
                Col1 = value
            };

            // Resultado
            var predict = EmoSense.Predict(prediccion).PredictedLabel;


            // Manejo
            return predict switch
            {
                0 => Types.Enumerations.Sentiments.Negative,
                1 => Types.Enumerations.Sentiments.Positive,
                4 => Types.Enumerations.Sentiments.Positive,
                _ => Types.Enumerations.Sentiments.Undefined
            };
        }
        catch (Exception ex)
        {
            ServerLogger.LogError("EmoSense: " + ex.Message);
        }

        return Types.Enumerations.Sentiments.Undefined;
    }



    /// <summary>
    /// Categoriza una imagen
    /// </summary>
    /// <param name="value">Imagen</param>
    public static ProductCategories Vision(byte[] value)
    {
        try
        {
            // Bitmap
            Bitmap image;
            using (MemoryStream ms = new(value))
            {
                image = new(ms);
            }

            // Prediccion
            var prediccion = new VisionIA.ModelInput()
            {
                ImageSource = image
            };

            // Resultado
            var predict = VisionIA.Predict(prediccion).Prediction;

            // Manejo
            ProductCategories categoria = ProductCategories.Undefined;
            categoria = predict switch
            {
                "Agricultura" => ProductCategories.Agricultura,
                "Alimentos" => ProductCategories.Alimentos,
                "Animales" => ProductCategories.Animales,
                "Arte" => ProductCategories.Arte,
                "Automoviles" => ProductCategories.Automóviles,
                "Deporte" => ProductCategories.Deporte,
                "Frutas" => ProductCategories.Frutas,
                "Hogar" => ProductCategories.Hogar,
                "Juguetes" => ProductCategories.Juguetes,
                "Limpieza" => ProductCategories.Limpieza,
                "Moda" => ProductCategories.Moda,
                "Salud" => ProductCategories.Salud,
                "Mascotas" => ProductCategories.Animales,
                "Tecnologia" => ProductCategories.Tecnología,
                _ => ProductCategories.Undefined
            };

            return categoria;

        }
        catch (Exception ex)
        {
            ServerLogger.LogError("Visionary IA: " + ex.Message);
        }

        return ProductCategories.Undefined;
    }



}