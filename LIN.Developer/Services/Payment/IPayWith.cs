namespace LIN.Developer.Services.Payment;


public interface IPayWith
{


    /// <summary>
    /// Metodo Pagar
    /// </summary>
    public Task<ResponseBase> Pay(TransactionDataModel transaccion);


}
