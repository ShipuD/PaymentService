using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentValidatorFactory : IPaymentValidatorFactory
    {
        public IPaymentValidator GetPaymentValidator(PaymentScheme paymentScheme)
        {
            switch (paymentScheme)
            {
                case PaymentScheme.Bacs:
                    return new BacsPaymentValidator();
                case PaymentScheme.FasterPayments:
                    return new FasterPaymentValidator();
                case PaymentScheme.Chaps:
                    return new ChapsPaymentValidator();
                default:
                    throw new ArgumentException($"No payment validator exists for {paymentScheme}");
            }
        }        
    }
}
