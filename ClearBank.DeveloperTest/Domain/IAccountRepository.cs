namespace ClearBank.DeveloperTest.Domain
{
    public interface IAccountRepository
    {
        void AddAccount(Account account);
        Account GetByAccountNumber(string accountNumber);
    }
}
