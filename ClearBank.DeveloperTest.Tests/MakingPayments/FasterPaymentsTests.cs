using ClearBank.DeveloperTest.Domain;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using FluentAssertions.Execution;
using System;
using System.Linq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.MakingPayments
{

    public class FasterPaymentsTests
    {
        private readonly PaymentsHarness harness;

        public FasterPaymentsTests()
        {
            harness = new PaymentsHarness();
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(100, 1)]
        [InlineData(100, 50)]
        public void ShouldReturnSuccessfulAndUpdateAccountBalance(decimal accountBalance, decimal payment)
        {
            var account = Account.Open(accountBalance);
            harness.AddAccount(account);


            var makePaymentRequest = CreateMakeFasterPaymentsPaymentRequest()
                with
            {
                DebtorAccountNumber = account.AccountNumber,
                Amount = payment
            };
            var result = harness.Service.MakePayment(makePaymentRequest);

            using var _ = new AssertionScope();
            result.Should().BeEquivalentTo(new
            {
                Success = true
            });
            account.Balance.Should().Be(accountBalance - payment);
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
        public void ShouldReturnUnSuccessfulWhenMakingPaymentForAccountsThatDontAllowFasterPaymentsSchemes()
        {
            var account = Account.Open(100m);
            account.DisableFasterPayments();
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

        [Theory]
        [InlineData(100, 101)]
        [InlineData(100, 102)]
        [InlineData(100, 103)]
        [InlineData(50, 100)]
        [InlineData(0, 1)]
        public void ShouldReturnUnSuccessfulWhenMakingPaymentForAccountsThatDontNotHaveEnoughFunds(decimal accountBalance, decimal payment)
        {
            var account = Account.Open(accountBalance);
            harness.AddAccount(account);


            var makePaymentRequest = CreateMakeFasterPaymentsPaymentRequest()
                with
            {
                DebtorAccountNumber = account.AccountNumber,
                Amount = payment
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
                PaymentScheme.FasterPayments
            );
            return makePaymentRequest;
        }

    }
}