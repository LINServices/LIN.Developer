namespace LIN.Developer.Services.Iam;


public class Resource
{


    
    /// <summary>
    /// Validar el acceso Iam a un recurso.
    /// </summary>
    /// <param name="profile">Id del perfil.</param>
    /// <param name="resourceId">Id del recurso.</param>
    public async Task<ReadOneResponse<IamLevels>> Validate(int profile, int resourceId)
    {

        // Obtiene el recurso.
        var resource = Data.Resources.Read(resourceId, profile);


    }



}