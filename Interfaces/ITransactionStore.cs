
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

public class TransactionStore : ITransactionStore
{
    private readonly IQueryable<Transaction> _transactions;

    public TransactionStore()
    {
        _transactions = new List<Transaction>().AsQueryable();
    }

    Task<long> ITransactionStore.Add(Guid referenceId, long accountNr, decimal amount, string payload)
    {
        long uniqueId = _transactions.Count() + 1;
        _transactions.Append(new Transaction { ReferenceId = referenceId, AccountNr = accountNr, Amount = amount, UniqueId = uniqueId });

        return Task.FromResult(uniqueId);
    }

    Task<Transaction> ITransactionStore.Get(long uniqueId)
    {
        Transaction transaction = _transactions.Where(tx => tx.UniqueId == uniqueId).First();

        return Task.FromResult(transaction);
    }

    async IAsyncEnumerable<Transaction> ITransactionStore.SearchBy(Guid referenceId)
    {
        await foreach (var transaction in _transactions.ToAsyncEnumerable())
        {
            if (transaction.ReferenceId == referenceId)
                yield return transaction;
        }
    }

    async IAsyncEnumerable<Transaction> ITransactionStore.SearchFrom(long uniqueId, long accountNr)
    {
        await foreach (var transaction in _transactions.Where(tx => tx.Amount == accountNr && tx.UniqueId == uniqueId).ToAsyncEnumerable())
        {
            yield return transaction;
        }
    }
}
