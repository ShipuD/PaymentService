using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClearBank.DeveloperTest.Services
{
    public interface IPaymentValidatorFactory
    {
        public IPaymentValidator GetPaymentValidator(PaymentScheme paymentScheme);
    }
}
