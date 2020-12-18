using ClearBank.DeveloperTest.Domain;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using FluentAssertions.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.MakingPayments
{
    public class BacsPaymentTests
    {
        private readonly PaymentsHarness harness;

        public BacsPaymentTests()
        {
            harness = new PaymentsHarness();
        }

        [Fact]
        public void ShouldReturnSuccessfulAndUpdateAccountBalance()
        {
            var balance = 150m;
            var account = Account.Open(balance);
            harness.AddAccount(account);

            var makePaymentRequest = CreateMakeBacsPaymentRequest()
                with
            {
                DebtorAccountNumber = account.AccountNumber,
                Amount = 50
            };
            var result = harness.Service.MakePayment(makePaymentRequest);

            using var _ = new AssertionScope();
            result.Should().BeEquivalentTo(new
            {
                Success = true
            });
            account.Balance.Should().Be(100);
        }



        [Fact]
        public void ShouldReturnUnSuccessfulWhenMakingPaymentForAccountThatDoesNotExist()
        {
            var makePaymentRequest = CreateMakeBacsPaymentRequest();

            var makePaymentResult = harness.Service.MakePayment(makePaymentRequest);

            using var _ = new AssertionScope();
            makePaymentResult
                .Should().BeEquivalentTo(new
                {
                    Success = false
                });
        }

        [Fact]
        public void ShouldReturnUnSuccessfulWhenMakingPaymentForAccountsThatDontAllowBacsPaymentSchemes()
        {
            var account = Account.Open(100m);
            account.DisableBacsPayments();
            harness.AddAccount(account);

            var makePaymentRequest = CreateMakeBacsPaymentRequest()
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

        private static MakePaymentRequest CreateMakeBacsPaymentRequest()
        {
            var makePaymentRequest = new MakePaymentRequest(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                10,
                DateTime.UtcNow,
                PaymentScheme.Bacs
            );
            return makePaymentRequest;
        }

    }
}