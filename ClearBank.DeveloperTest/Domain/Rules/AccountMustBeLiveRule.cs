namespace ClearBank.DeveloperTest.Domain.Rules
{
    public class AccountMustBeLiveRule : IRule
    {
        private readonly AccountStatus accountStatus;

        public AccountMustBeLiveRule(AccountStatus accountStatus)
        {
            this.accountStatus = accountStatus;
        }

        public string Message => "Account must be live";

        public bool IsBroken()
        {
            return accountStatus != AccountStatus.Live;
        }
    }
}
