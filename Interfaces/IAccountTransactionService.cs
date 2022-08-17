public interface IAccountTransactionService
{
    Task DepositWithdrawal(Guid referenceId, long accountNr, decimal amount);
    Task Transfer(Guid referenceId, long accountNrFrom, long accountNrTo, decimal amount);
}
