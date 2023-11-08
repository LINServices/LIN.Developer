namespace LIN.Developer.Services.Iam;


public class Resource
{


    /// <summary>
    /// Validar el acceso Iam a un recurso.
    /// </summary>
    /// <param name="profile">Id del perfil.</param>
    /// <param name="resourceId">Id del recurso.</param>
    public static async Task<ReadOneResponse<IamLevels>> Validate(int profile, string resourceId)
    {
        try
        {
            // Conexión con MongoDB.
            var mongo = MongoService.GetOneConnection();

            // Obtiene la colección.
            var collection = mongo.DataBase.GetCollection<ResourceModel>("projects");

            // Filtros.
            var filter = Builders<ResourceModel>.Filter.Eq("_id", new ObjectId(resourceId));
            var projection = Builders<ResourceModel>.Projection.ElemMatch(r => r.Allowed, a => a.Profile == profile);

            // Resultado.
            var result = await collection.Find(filter).Project(projection).FirstOrDefaultAsync();

            // Convertir a Json.
            var allowedElement = result.GetValue("Allowed").AsBsonArray.FirstOrDefault().ToJson() ?? "";

            // Modelo de acceso.
            AccessModel? access = System.Text.Json.JsonSerializer.Deserialize<AccessModel>(allowedElement);

            // Si no existe el acceso.
            if (access == null)
                return new()
                {
                    Response = Responses.NotRows,
                    Model = IamLevels.NotAccess
                };

            // Respuesta.
            return new()
            {
                Response = Responses.Success,
                Model = access.IamLevel
            };

        }
        catch (Exception ex)
        {
            return new()
            {
                Response = Responses.Undefined,
                Message = ex.Message
            };
        }

    }



}