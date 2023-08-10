using LIN.Types.Developer.Models;

namespace LIN.Developer.Services.Payment;


public class PayToken : IPayWith
{

    /// <summary>
    /// Guarda el token
    /// </summary>
    private readonly string _token;


    /// <summary>
    /// Si el token es valido
    /// </summary>
    private bool _isValid;


    /// <summary>
    /// ID del perfil dev
    /// </summary>
    private int _profile;



    /// <summary>
    /// Nuevo pago por token
    /// </summary>
    /// <param name="token">Token</param>
    public PayToken(string token)
    {
        this._token = token;
        Validate();
    }



    /// <summary>
    /// Valida el token 
    /// </summary>
    private void Validate()
    {

        var (isValid, account, profile) = Jwt.Validate(this._token);

        if (isValid)
        {
            _isValid = true;
            _profile = profile;
            return;
        }

        _isValid = false;

    }



    /// <summary>
    /// Realiza el cobro
    /// </summary>
    public async Task<ResponseBase> Pay(TransactionDataModel transaccion)
    {

        // Token invalido
        if (!_isValid)
        {
            return new()
            {
               Message = "El token a caducado o es invalido",
               Response = Responses.Unauthorized
            };
        }

       
        // ID del perfil
        transaccion.ProfileID = _profile;
        transaccion.Valor = Pricing.ToNegative(transaccion.Valor);

        // Efectua
        var res = await Data.Transactions.Generate(transaccion);


        // Validacion
        var response = new ResponseBase()
        {
            Response = res.Response,
            Message = (res.Response == Responses.Success) ? "" : "No se pudo efectuar el cobro."
        };

        return response;
    }


}