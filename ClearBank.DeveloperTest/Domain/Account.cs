using ClearBank.DeveloperTest.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClearBank.DeveloperTest.Domain
{
    public class Account
    {
        public string AccountNumber { get; }
        public decimal Balance { get; private set; }
        public AccountStatus Status { get; private set; }
        public List<PaymentScheme> AllowedPaymentSchemes { get; private set; }

        private Account(decimal balance)
        {
            Random rnd = new Random();
            int myRandomNo = rnd.Next(10000000, 99999999);
            AccountNumber = myRandomNo.ToString();
            Balance = balance;
            Status = AccountStatus.Live;
            AllowedPaymentSchemes = new List<PaymentScheme>
            {
                PaymentScheme.Bacs,
                PaymentScheme.Chaps,
                PaymentScheme.FasterPayments
            };
        }

        public static Account Open(decimal balance)
        {
            return new Account(balance);
        }

        public void MakePayment(decimal amount, PaymentScheme paymentScheme)
        {
            CheckRule(new AccountMustBeLiveRule(Status));
            CheckRule(new MustBeValidPaymentSchemeRule(AllowedPaymentSchemes, paymentScheme));
            Balance -= amount;
            CheckRule(new MustHaveSufficientFundsRule(Balance));
        }

        public void SetAccountLive()
        {
            Status = AccountStatus.Live;
        }

        public void InboundPaymentsOnly()
        {
            Status = AccountStatus.InboundPaymentsOnly;
        }

        public void DisableAccount()
        {
            Status = AccountStatus.Disabled;
        }

        public void EnableFasterPayments()
        {
            if (!AllowedPaymentSchemes.Contains(PaymentScheme.FasterPayments)) AllowedPaymentSchemes.Add(PaymentScheme.FasterPayments);
        }

        public void DisableFasterPayments()
        {
            var fasterPayments = AllowedPaymentSchemes.RemoveAll(x => x == PaymentScheme.FasterPayments);
        }

        public void EnableBacsPayments()
        {
            if (!AllowedPaymentSchemes.Contains(PaymentScheme.Bacs)) AllowedPaymentSchemes.Add(PaymentScheme.Bacs);
        }

        public void DisableBacsPayments()
        {
            var bacsPayments = AllowedPaymentSchemes.RemoveAll(x => x == PaymentScheme.Bacs);
        }

        public void EnableChapsPayments()
        {
            if (!AllowedPaymentSchemes.Contains(PaymentScheme.Chaps)) AllowedPaymentSchemes.Add(PaymentScheme.Chaps);
        }

        public void DisableChapsPayments()
        {
            var chapsPayments = AllowedPaymentSchemes.RemoveAll(x => x == PaymentScheme.Chaps);
        }


        private void CheckRule(IRule rule)
        {
            if (rule.IsBroken())
            {
                throw new RuleBrokenException(rule);
            }
        }
    }
}
