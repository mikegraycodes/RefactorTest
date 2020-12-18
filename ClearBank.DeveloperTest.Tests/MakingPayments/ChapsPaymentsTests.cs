using ClearBank.DeveloperTest.Domain;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using FluentAssertions.Execution;
using System;
using System.Linq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.MakingPayments
{
    public class ChapsPaymentsTests
    {
        private readonly PaymentsHarness harness;

        public ChapsPaymentsTests()
        {
            harness = new PaymentsHarness();
        }

        [Fact]
        public void ShouldReturnSuccessfulAndUpdateAccountBalance()
        {
            var account = Account.Open(100m);
            harness.AddAccount(account);


            var makePaymentRequest = CreateMakeFasterPaymentsPaymentRequest()
                with
            {
                DebtorAccountNumber = account.AccountNumber,
                Amount = 25
            };
            var result = harness.Service.MakePayment(makePaymentRequest);

            using var _ = new AssertionScope();
            result.Should().BeEquivalentTo(new
            {
                Success = true
            });
            account.Balance.Should().Be(75);
        }

        [Fact]
        public void ShouldReturnUnSuccessfulAndNotUpdateAccountsWhenNoAccountExits()
        {
            var makePaymentRequest = CreateMakeFasterPaymentsPaymentRequest();
            var result = harness.Service.MakePayment(makePaymentRequest);

            using var _ = new AssertionScope();
            result.Should().BeEquivalentTo(new
            {
                Success = false
            });
        }

        [Fact]
        public void ShouldReturnUnSuccessfulWhenMakingPaymentForAccountsThatDontAllowChapsPaymentsSchemes()
        {
            var account = Account.Open(100m);
            account.DisableChapsPayments();
            harness.AddAccount(account);

            var makePaymentRequest = CreateMakeFasterPaymentsPaymentRequest()
                with
            {
                DebtorAccountNumber = account.AccountNumber
            };

            var result = harness.Service.MakePayment(makePaymentRequest);

            using var _ = new AssertionScope();
            result
                .Should().BeEquivalentTo(new
                {
                    Success = false
                });
        }


        [Fact]
        public void ShouldReturnUnSuccessfulWhenAccountStatusIsNotLive()
        {
            var account = Account.Open(100m);
            account.DisableAccount();
            harness.AddAccount(account);


            var makePaymentRequest = CreateMakeFasterPaymentsPaymentRequest()
                with
            {
                DebtorAccountNumber = account.AccountNumber
            };

            var result = harness.Service.MakePayment(makePaymentRequest);

            using var _ = new AssertionScope();
            result
                .Should().BeEquivalentTo(new
                {
                    Success = false
                });
        }

        private static MakePaymentRequest CreateMakeFasterPaymentsPaymentRequest()
        {
            var makePaymentRequest = new MakePaymentRequest(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                10,
                DateTime.UtcNow,
                PaymentScheme.Chaps
            );
            return makePaymentRequest;
        }
    }
}