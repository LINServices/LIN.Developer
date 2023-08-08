namespace LIN.Developer.Data.IA;


public static class IA
{


    /// <summary>
    /// Categoriza el idioma de un string
    /// </summary>
    /// <param name="value">Texto</param>
    public static LangEnum Lang(string value)
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
                0 => LangEnum.Spain,
                1 => LangEnum.English,
                _ => LangEnum.Undefined,
            };
        }
        catch (Exception ex)
        {
            ServerLogger.LogError("Lingua IA: " + ex.Message);
        }

        return LangEnum.Undefined;
    }



    /// <summary>
    /// Categoriza el genero de un nombre
    /// </summary>
    /// <param name="value">Nombre</param>
    public static Sexos Gender(string value)
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
                0 => Sexos.Female,
                1 => Sexos.Male,
                _ => Sexos.Undefined,
            };
        }
        catch (Exception ex)
        {
            ServerLogger.LogError("Genderize: " + ex.Message);
        }

        return Sexos.Undefined;
    }



    /// <summary>
    /// Categoriza el sentimiento de un texto
    /// </summary>
    /// <param name="value">Texto</param>
    public static Sentiment Sentiment(string value)
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
                0 => Shared.Enumerations.Sentiment.Negative,
                1 => Shared.Enumerations.Sentiment.Positive,
                4 => Shared.Enumerations.Sentiment.Positive,
                _ => Shared.Enumerations.Sentiment.Undefined
            };
        }
        catch (Exception ex)
        {
            ServerLogger.LogError("EmoSense: " + ex.Message);
        }

        return Shared.Enumerations.Sentiment.Undefined;
    }



    /// <summary>
    /// Categoriza el sentimiento de un texto
    /// </summary>
    /// <param name="value">Texto</param>
    public static Sentiment EmoSenses(string value)
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
                0 => Shared.Enumerations.Sentiment.Negative,
                1 => Shared.Enumerations.Sentiment.Positive,
                4 => Shared.Enumerations.Sentiment.Positive,
                _ => Shared.Enumerations.Sentiment.Undefined
            };
        }
        catch (Exception ex)
        {
            ServerLogger.LogError("EmoSense: " + ex.Message);
        }

        return Shared.Enumerations.Sentiment.Undefined;
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
                "Tecnologia" => ProductCategories.Tecnologia,
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