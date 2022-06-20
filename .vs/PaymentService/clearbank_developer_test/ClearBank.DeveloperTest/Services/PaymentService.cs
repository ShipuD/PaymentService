using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using System.Configuration;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStore _accountDataStore;
        private readonly IPaymentValidatorFactory _paymentValidatorFactory;

        public PaymentService(IAccountDataStore accountDataStore,
            IPaymentValidatorFactory paymentValidatorFactory)
        {  
            _accountDataStore = accountDataStore;
            _paymentValidatorFactory = paymentValidatorFactory;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var result = new MakePaymentResult
            {
                Success = false
            };

         
            var account = _accountDataStore.GetAccount(request.DebtorAccountNumber);

            var paymentValidator = _paymentValidatorFactory.GetPaymentValidator(request.PaymentScheme);

            bool isValid = paymentValidator.ValidatePayment(request, account);

            if(!isValid)
            {
                return result;
            }

            account.Balance -= request.Amount;

            _accountDataStore.UpdateAccount(account);

            result.Success = true;

            return result;
        }
    }
}
