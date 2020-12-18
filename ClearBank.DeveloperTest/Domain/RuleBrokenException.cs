using System;

namespace ClearBank.DeveloperTest.Domain
{
    public class RuleBrokenException : Exception
    {
        public IRule BrokenRule { get; }

        public string Details { get; }

        public RuleBrokenException(IRule brokenRule) : base(brokenRule.Message)
        {
            BrokenRule = brokenRule;
            this.Details = brokenRule.Message;
        }

        public override string ToString()
        {
            return $"{BrokenRule.GetType().FullName}: {BrokenRule.Message}";
        }
    }
}
