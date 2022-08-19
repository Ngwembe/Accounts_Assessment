public class AccountStore : IAccountStore
{
    private readonly IQueryable<Account> _accounts;
    private readonly ITransactionStore _transactionStore;

    public AccountStore(ITransactionStore transactionStore)
    {
        _accounts = new List<Account>()
        { 
            new Account() { Balance = 8000, TransactionMarker = "Genesis", AccountNr = 100001 } 
        }.AsQueryable();
               
        _transactionStore = transactionStore;
    }

    /// <summary>
    /// Returns all registered accounts
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async IAsyncEnumerable<long> GetAccounts()
    {
        await foreach(var account in _accounts.ToAsyncEnumerable())
        {
            yield return account.AccountNr;
        }
    }

    /// <summary>
    /// Retrieves the balance together with a transaction marker value for the given account
    /// </summary>
    /// <param name="accountNr"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<(decimal balance, string txMarker)> GetBalance(long accountNr)
    {
        var account = await _accounts.Where(acc => acc.AccountNr == accountNr)
                                     .ToAsyncEnumerable().FirstAsync();

        if (account == null)
            throw new Exception($"Account with account number: {accountNr} not found.");

        return (account.Balance, account.TransactionMarker);
    }

    /// <summary>
    /// Sets the balance of the given account number together with the transaction marker value
    /// </summary>
    /// <param name="accountNr"></param>
    /// <param name="balance"></param>
    /// <param name="txMarker"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task SetBalance(long accountNr, decimal balance, string txMarker)
    {
        var account = await _accounts.Where(acc => acc.AccountNr == accountNr).ToAsyncEnumerable().FirstAsync();

        if (account == null)
            throw new Exception($"Account with account number: {accountNr} not found.");

        account.Balance = balance;
        account.TransactionMarker = txMarker;
    }
}
