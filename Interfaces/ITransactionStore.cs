
public interface ITransactionStore
{
    /// <summary>
    ///  Returns primary unique id
    /// </summary>
    /// <param name="referenceId"></param>
    /// <param name="accountNr"></param>
    /// <param name="amount"></param>
    /// <param name="payload"></param>
    /// <returns></returns>
    Task<long> Add(Guid referenceId, long accountNr, decimal amount, string payload);

    Task<Transaction> Get(long uniqueId);

    //  Retrieves all transactions for the given account from the uniqueId (including)
    IAsyncEnumerable<Transaction> SearchFrom(long uniqueId, long accountNr);

    //  Retrieves all transactions with the given reference id
    IAsyncEnumerable<Transaction> SearchBy(Guid referenceId);
}
