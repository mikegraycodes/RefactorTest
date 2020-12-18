using System.Collections.Generic;

namespace ClearBank.DeveloperTest.Domain.Rules
{
    public class MustBeValidPaymentSchemeRule : IRule
    {
        private readonly IList<PaymentScheme> allowedPaymentSchemes;
        private readonly PaymentScheme paymentMethod;

        public MustBeValidPaymentSchemeRule(IList<PaymentScheme> allowedPaymentSchemes, PaymentScheme paymentMethod)
        {
            this.allowedPaymentSchemes = allowedPaymentSchemes;
            this.paymentMethod = paymentMethod;
        }

        public string Message => "Must be valid payment scheme";

        public bool IsBroken()
        {
            return !allowedPaymentSchemes.Contains(paymentMethod);
        }
    }
}
