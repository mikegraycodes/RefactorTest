using ClearBank.DeveloperTest.Domain;
using ClearBank.DeveloperTest.Services;
using System.Collections.Generic;

namespace ClearBank.DeveloperTest.Tests.MakingPayments
{
    public class PaymentsHarness : IAccountRepository, IUnitOfWork
    {
        public PaymentService Service { get; }
        private readonly Dictionary<string, Account> accounts = new();


        public PaymentsHarness()
        {
            Service = new PaymentService(this, this);
        }


        public Account GetByAccountNumber(string accountNumber)
        {
            return accounts.TryGetValue(accountNumber, out var account)
                is true
                ? account
                : null;
        }

        public void AddAccount(Account account)
        {
            accounts.Add(account.AccountNumber, account);
        }

        public void Complete()
        {
            //
        }
    }
}