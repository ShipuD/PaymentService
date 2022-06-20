using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClearBank.DeveloperTest.Services
{
    public class FasterPaymentValidator : IPaymentValidator
    {
        public bool ValidatePayment(MakePaymentRequest request, Account account)
        {
            return (account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments) &&
                   (account.Balance > request.Amount));
        }
    }
}
