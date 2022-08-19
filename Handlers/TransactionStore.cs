
public class TransactionStore : ITransactionStore
{
    private readonly IList<Transaction> _transactions;

    public TransactionStore()
    {
        _transactions = new List<Transaction>();
    }

    Task<long> ITransactionStore.Add(Guid referenceId, long accountNr, decimal amount, string payload)
    {
        long uniqueId = _transactions.Count() + 1;
        _transactions.Add(new Transaction { ReferenceId = referenceId, AccountNr = accountNr, Amount = amount, UniqueId = uniqueId });

        return Task.FromResult(uniqueId);
    }

    Task<Transaction> ITransactionStore.Get(long uniqueId)
    {
        Transaction transaction = _transactions.Where(tx => tx.UniqueId == uniqueId)
                                               .AsQueryable()
                                               .First();

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
