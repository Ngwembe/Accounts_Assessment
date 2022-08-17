public class Account
{
    private long _accountNr;
    public long AccountNr
    {
        get { return _accountNr; }
        //set { _accountNr = value; }
    }


    private decimal _balance;
    public decimal Balance
    {
        get { return _balance; }
        set { _balance = value; }
    }

    private string _transactionMarker;
    public string TransactionMarker
    {
        get { return _transactionMarker; }
        set { _transactionMarker = value; }
    }
}
