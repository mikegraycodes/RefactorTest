namespace ClearBank.DeveloperTest.Domain.Rules
{
    public class MustHaveSufficientFundsRule : IRule
    {
        private readonly decimal balance;

        public MustHaveSufficientFundsRule(decimal balance)
        {
            this.balance = balance;
        }

        public string Message => "Balance must be great than zero";

        public bool IsBroken() => balance < 0;
    }
}
