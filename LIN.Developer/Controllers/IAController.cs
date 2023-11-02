using LIN.Developer.Services.Payment;
using LIN.Types.Inventory.Enumerations;
namespace LIN.Developer.Controllers;


[Route("IA")]
public class IAController : ControllerBase
{


    /// <summary>
    /// Predicción del idioma de un texto
    /// </summary>
    /// <param name="content">Texto</param>
    /// <param name="apiKey">ApiKey</param>
    /// <param name="http">Contexto HTTP</param>
    [HttpPost("predict/Lang")]
    public async Task<HttpReadOneResponse<Languajes>> LangPredict([FromBody] string content, [FromHeader] string apiKey, [FromServices] IHttpContextAccessor http)
    {
        try
        {

            Services.Http.GetPlatform(http);

            // Conexión a BD
            var (context, contextKey) = Conexión.GetOneConnection();

            // Obtiene la IP
            var myIP = Firewall.HttpIPv4(http);

            // Evalúa el firewall
            var evaluación = await Firewall.EvaluateFirewall(apiKey, context, myIP);

            // Respuesta
            if (evaluación.Response != Responses.Success)
            {
                context.CloseActions(contextKey);
                var value = new ReadOneResponse<Languajes>(Responses.Unauthorized, Languajes.Undefined)
                {
                    Message = evaluación.Message
                };

                return value;
            }

            // Modelo del USO
            ApiKeyUsesDataModel uso = new()
            {
                ID = 0,
                Valor = Abstractions.PriceTable.LangIA
            };

            // realiza el cobro
            var result = await Data.ApiKeyUses.GenerateUses(uso, apiKey, context);

            // Cierra la conexión
            context.CloseActions(contextKey);

            // Evalúa el cobro
            if (result.Response != Responses.Success)
            {
                var responseA = new ReadOneResponse<Languajes>()
                {
                    Response = result.Response,
                    Message = "No se pudo efectuar el cobro."
                };
                return responseA;
            }


            // Manejo
            var lang = Data.IA.IA.Lang(content);

            // Respuesta
            var response = new ReadOneResponse<Languajes>()
            {
                Response = Responses.Success,
                Model = lang
            };

            return response;
        }
        catch
        {
        }
        // Respuesta fatal
        var responseFatal = new ReadOneResponse<Languajes>()
        {
            Response = Responses.Undefined,
            Model = Languajes.Undefined,
            Message = "Hubo un error en el servidor, intenta mas tarde"
        };

        return responseFatal;

    }



    /// <summary>
    /// Predicción del genero de un nombre
    /// </summary>
    /// <param name="name">Nombre</param>
    /// <param name="apiKey">ApiKey</param>
    /// <param name="http">Contexto HTTP</param>
    [HttpPost("predict/Name")]
    public async Task<HttpReadOneResponse<Genders>> NamePredict([FromBody] string name, [FromHeader] string apiKey, [FromServices] IHttpContextAccessor http)
    {
        try
        {
            Services.Http.GetPlatform(http);
            // Conexión a BD
            var (context, contextKey) = Conexión.GetOneConnection();

            // Obtiene la IP
            var myIP = Firewall.HttpIPv4(http);

            // Evalúa el firewall
            var evaluation = await Firewall.EvaluateFirewall(apiKey, context, myIP);

            // Respuesta
            if (evaluation.Response != Responses.Success)
            {
                context.CloseActions(contextKey);
                var value = new ReadOneResponse<Genders>(Responses.Unauthorized, Genders.Undefined)
                {
                    Message = evaluation.Message
                };

                return value;
            }

            // Modelo del USO
            ApiKeyUsesDataModel uso = new()
            {
                ID = 0,
                Valor = Abstractions.PriceTable.NamesIA
            };

            // realiza el cobro
            var result = await Data.ApiKeyUses.GenerateUses(uso, apiKey, context);

            context.CloseActions(contextKey);

            // Evalúa el cobro
            if (result.Response != Responses.Success)
            {
                var responseA = new ReadOneResponse<Genders>()
                {
                    Response = result.Response,
                    Message = "No se pudo efectuar el cobro."
                };
                return responseA;
            }

            // Manejo
            var sex = Data.IA.IA.Gender(name);


            var response = new ReadOneResponse<Genders>()
            {
                Response = Responses.Success,
                Model = sex,
            };

            return response;
        }
        catch
        {
        }

        var responseFatal = new ReadOneResponse<Genders>()
        {
            Response = Responses.Undefined,
            Model = Genders.Undefined,
            Message = "Hubo un error en el servidor, intenta mas tarde"
        };
        return responseFatal;

    }



    /// <summary>
    /// Predicción del sentimiento de un texto
    /// </summary>
    /// <param name="text">Texto a analizar</param>
    /// <param name="apiKey">ApiKey</param>
    /// <param name="http">Contexto HTTP</param>
    [HttpPost("predict/Sentiment")]
    public ActionResult<HttpReadOneResponse<Sentiments>> SentimentPredict([FromBody] string text, [FromHeader] string apiKey, [FromServices] IHttpContextAccessor http)
    {
        try
        {
            Services.Http.GetPlatform(http);
            //Load sample data
            var sampleData = new SentimentIA.ModelInput()
            {
                Col1 = text.Normalize().Trim().ToLower(),
            };

            //Load model and predict output
            var ia = Data.IA.IA.Sentiment(text);

            // Manejo
            var response = new ReadOneResponse<Sentiments>()
            {
                Response = Responses.Success,
                Model = ia,
            };
            return StatusCode(200, response);
        }
        catch
        {
        }


        var responseFatal = new ReadOneResponse<Sentiments>()
        {
            Response = Responses.Undefined,
            Model = Sentiments.Undefined,
            Message = "Hubo un error en el servidor, intenta mas tarde"
        };
        return StatusCode(500, responseFatal);

    }



    /// <summary>
    /// Predicción de la categoría de una imagen
    /// </summary>
    /// <param name="imageByte">Imagen en bytes</param>
    /// <param name="apiKey">ApiKey</param>
    /// <param name="http">Contexto HTTP</param>
    [HttpPost("predict/Vision")]
    public async Task<HttpReadOneResponse<ProductCategories>> VisionPredict([FromBody] byte[] imageByte, [FromHeader] string apiKey, [FromServices] IHttpContextAccessor http)
    {

        Services.Http.GetPlatform(http);

        // Conexión a BD
        var (context, contextKey) = Conexión.GetOneConnection();

        // Obtiene la IP
        var myIP = Firewall.HttpIPv4(http);

        // Evalúa el firewall
        var evaluation = await Firewall.EvaluateFirewall(apiKey, context, myIP);

        // Respuesta
        if (evaluation.Response != Responses.Success)
        {
            context.CloseActions(contextKey);
            return new ReadOneResponse<ProductCategories>(evaluation.Response, ProductCategories.Undefined)
            {
                Message = evaluation.Message
            };
        }


        // Parámetro invalido
        if (imageByte == null || imageByte.Length <= 0 || apiKey == null || apiKey.Length < 15)
            return new(Responses.InvalidParam);

        try
        {

            // Modelo del USO
            ApiKeyUsesDataModel uso = new()
            {
                ID = 0,
                Valor = Abstractions.PriceTable.VisionIA
            };

            // Genera el uso
            var cobro = await Data.ApiKeyUses.GenerateUses(uso, apiKey);


            // Si no se realizo el cobro
            if (cobro.Response != Responses.Success)
                return new(cobro.Response);

            // --
            ProductCategories categoría = Data.IA.IA.Vision(imageByte);

            if (categoría != ProductCategories.Undefined)
                return new(Responses.Success, categoría);

            return new(Responses.Undefined);

        }
        catch (Exception ex)
        {
            ServerLogger.LogError(ex.Message);
        }

        return new(Responses.Undefined, ProductCategories.Undefined);

    }



    /// <summary>
    /// Categoriza el sentimiento y el idioma de un texto
    /// </summary>
    /// <param name="pay">Método de pago</param>
    /// <param name="content">Contenido a categorizar</param>
    /// <param name="access">ApiKey || Token</param>
    /// <param name="http">Contexto HTTP</param>
    [HttpPost("Categorize/predict/{pay}")]
    public async Task<HttpReadOneResponse<CategorizeModel>> CategorizePredict([FromRoute] PayWith pay, [FromBody] string content, [FromHeader] string access, [FromServices] IHttpContextAccessor http)
    {
        try
        {
            Services.Http.GetPlatform(http);
            // Pagar con
            IPayWith payWith = (pay == PayWith.Key)
                               ? new PayKey(access, http)
                               : new PayToken(access);

            // transacción
            var transacción = new TransactionDataModel
            {
                Description = "Usada en LIN IA",
                Fecha = DateTime.Now,
                Tipo = TransactionTypes.UsedService,
                Valor = 8m
            };

            // Respuesta
            var responseCobro = await payWith.Pay(transacción);

            // Evaluación de respuesta
            switch (responseCobro.Response)
            {
                case Responses.Success:
                    break;

                default:
                    return new ReadOneResponse<CategorizeModel>()
                    {
                        Message = responseCobro.Message,
                        Response = responseCobro.Response,
                    };
            }


            // Predicción
            content = content.Normalize().Trim().ToLower();


            var lang = Data.IA.IA.Lang(content);
            var sentiment = Data.IA.IA.Sentiment(content);

            // respuesta final
            var response = new ReadOneResponse<CategorizeModel>()
            {
                Response = Responses.Success,
                Model = new()
                {
                    Languaje = lang,
                    Sentiment = sentiment
                }
            };

            return response;
        }
        catch
        {
        }


        var responseFatal = new ReadOneResponse<CategorizeModel>()
        {
            Response = Responses.Undefined,
            Model = new()
            {
                Languaje = Languajes.Undefined,
                Sentiment = Sentiments.Undefined
            },
            Message = "Hubo un error en el servidor, intenta mas tarde"
        };
        return responseFatal;

    }


}