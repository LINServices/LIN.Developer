namespace LIN.Developer.Services.Payment;


public interface IPayWith
{


    /// <summary>
    /// Método Pagar
    /// </summary>
    public Task<ResponseBase> Pay(TransactionDataModel transaction);


}
