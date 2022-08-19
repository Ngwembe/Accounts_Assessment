public class Account
{
    public Account()
    {
        _transactionMarkers = new List<string>();
    }

    private long _accountNr;
    public long AccountNr
    {
        get { return _accountNr; }
        set { _accountNr = value; }
    }


    private decimal _balance;
    public decimal Balance
    {
        get { return _balance; }
        set { _balance = value; }
    }

    private IList<string> _transactionMarkers;
    public string TransactionMarker
    {
        get { return _transactionMarkers[_transactionMarkers.Count - 1] ?? String.Empty; }
        set { _transactionMarkers.Add(value); }
    }
}
