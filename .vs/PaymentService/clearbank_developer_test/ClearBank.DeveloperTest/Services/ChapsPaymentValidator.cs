using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClearBank.DeveloperTest.Services
{
    public class ChapsPaymentValidator : IPaymentValidator
    {
        public bool ValidatePayment(MakePaymentRequest request, Account account)
        {
            return (account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps) &&
                   (account.Balance > request.Amount) &&
                   (account.Status == AccountStatus.Live));
        }
    }
}
