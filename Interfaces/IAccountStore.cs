public interface IAccountStore
{
    /// <summary>
    ///  Sets the balance of the given account number together with the transaction marker value
    /// </summary>
    /// <param name="accountNr"></param>
    /// <param name="balance"></param>
    /// <param name="txMarker"></param>
    /// <returns></returns>
    Task SetBalance(long accountNr, decimal balance, string txMarker);

    /// <summary>
    ///  Retrieves the balance together with a transaction marker value for the given account
    /// </summary>
    /// <param name="accountNr"></param>
    /// <returns></returns>
    Task<(decimal balance, string txMarker)> GetBalance(long accountNr);

    /// <summary>
    ///  Returns all registered accounts
    /// </summary>
    /// <returns></returns>
    IAsyncEnumerable<long> GetAccounts();
}
