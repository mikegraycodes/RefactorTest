using ClearBank.DeveloperTest.Domain;
using System;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.Infrastructure
{
    public class AccountRepository : IAccountRepository
    {
        public void AddAccount(Account account)
        {
            throw new NotImplementedException();
        }

        public Account GetByAccountNumber(string accountNumber)
        {
            throw new NotImplementedException();
        }
    }
}
