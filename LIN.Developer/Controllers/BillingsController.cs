﻿using LIN.Developer.Data.Sql;

namespace LIN.Developer.Controllers;


[Route("billing")]
public class BillingsController : Controller
{


    /// <summary>
    /// Obtiene la lista de movimientos asociados a un perfil
    /// </summary>
    /// <param name="id">ID del perfil</param>
    [HttpGet("read/all")]
    public async Task<HttpReadAllResponse<TransactionDataModel>> ReadAll([FromHeader] string token)
    {

        // Token
        var (isValid, _, profile) = Jwt.Validate(token);

        // Validación de token
        if (!isValid)
            return new(Responses.Unauthorized);

        // Obtiene el usuario
        var response = await Transactions.ReadAll(profile);

        // Retorna el resultado
        return response;

    }



    /// <summary>
    /// Genera una transacción
    /// </summary>
    /// <param name="token">Token de acceso</param>
    /// <param name="amount">Monto</param>
    [HttpPost("generate")]
    public async Task<HttpResponseBase> Generate([FromHeader] string token, [FromHeader] decimal amount)
    {

        // Obtiene el usuario
        var (isValid, _, profile) = Jwt.Validate(token);

        if (!isValid)
            return new(Responses.Unauthorized);
        
        var transaction = new TransactionDataModel()
        {
            Fecha = DateTime.Now,
            Valor = amount,
            Tipo = TransactionTypes.UsedService,
            Profile = new()
            {
                ID = profile
            },
            Description = "Usado en LIN Apps"
        };
        var response = await Transactions.Generate(transaction);

        // Retorna el resultado
        return response;

    }


}