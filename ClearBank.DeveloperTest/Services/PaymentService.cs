using ClearBank.DeveloperTest.Domain;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IUnitOfWork unitOfWork;

        public PaymentService(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
        {
            this.accountRepository = accountRepository ?? throw new System.ArgumentNullException(nameof(accountRepository));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var account = accountRepository.GetByAccountNumber(request.DebtorAccountNumber);

            if (account == null)
            {
                return MakePaymentResult.CreateFail();
            }

            try
            {
                account.MakePayment(request.Amount, request.PaymentScheme);
                unitOfWork.Complete();
                return MakePaymentResult.CreateSuccess();
            }
            catch (RuleBrokenException)
            {
                return MakePaymentResult.CreateFail();
            }
        }
    }
}