using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClearBank.DeveloperTest.Services
{
    public interface IPaymentValidator
    {
        public bool ValidatePayment(MakePaymentRequest request, Account account);
    }
}
