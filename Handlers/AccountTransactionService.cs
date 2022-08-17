﻿public class AccountTransactionService : IAccountTransactionService
{
    private readonly IAccountStore _accountStore;
    private readonly ITransactionStore _transactionStore;

    public AccountTransactionService(IAccountStore accountStore, ITransactionStore transactionStore)
    {
        _accountStore = accountStore;
        _transactionStore = transactionStore;
    }

    async Task IAccountTransactionService.DepositWithdrawal(Guid referenceId, long accountNr, decimal amount)
    {
        try
        {
            await _accountStore.GetBalance(accountNr).ContinueWith(async result =>
            {
                var balance = result.Result.balance;
                var txMarker = result.Result.txMarker;

                long uniqueId = await _transactionStore.Add(referenceId, accountNr, amount, txMarker);

                //  Assuming the caller already set 'balance' as either positive for a deposit or negative for a withdrawal
                await _accountStore.SetBalance(accountNr, balance, txMarker);
            });
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    async Task IAccountTransactionService.Transfer(Guid referenceId, long accountNrFrom, long accountNrTo, decimal amount)
    {
        try
        {
            //  Will ensure only a single thread can execute the transfer codeblock
            var semaphore = new SemaphoreSlim(1);

            await semaphore.WaitAsync();
            var uniqueId = await _transactionStore.Add(referenceId, accountNrFrom, -amount, string.Empty);

            Task? setSenderBalance = null;
            Task? setRecipientBalance = null;
            decimal senderBalance = 0;
            decimal recipientBalance = 0;
            var senderAccountRetrievalTask = await _accountStore.GetBalance(accountNrFrom).ContinueWith(senderAccount => {
                if (senderAccount == null)
                    throw new ArgumentException("The sender's account was not found.");

                senderBalance = senderAccount.Result.balance;
                if (senderBalance < amount)
                    throw new InvalidOperationException($"The current balance is lower than the amount to transfer.\nCurrent Balance: R{senderBalance}\nAmount to transfer: R{amount}");
                return Task.CompletedTask;
            }, new CancellationToken(true));

            var recipientAccountRetrievalTask = await _accountStore.GetBalance(accountNrTo).ContinueWith(recipientAccount => {
                if (recipientAccount == null)
                    throw new ArgumentException("The recipient account was not found.");

                recipientBalance = recipientAccount.Result.balance;
                return Task.CompletedTask;
            }, new CancellationToken(true));

            if (senderBalance > 0)
            {
                setSenderBalance = Task.Run(async () => await _accountStore.SetBalance(accountNrFrom, senderBalance - amount, uniqueId.ToString()));
                setRecipientBalance = Task.Run(async () => await _accountStore.SetBalance(accountNrTo, (recipientBalance + amount), uniqueId.ToString()));
            }

            if (setRecipientBalance != null && setSenderBalance != null)
                Task.WaitAll(new Task[] { senderAccountRetrievalTask, recipientAccountRetrievalTask, setRecipientBalance, setSenderBalance });

            semaphore.Release();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}