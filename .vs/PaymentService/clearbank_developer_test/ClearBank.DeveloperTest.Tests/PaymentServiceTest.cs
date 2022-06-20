using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClearBank.DeveloperTest.Tests
{
    [TestClass]
    public class PaymentServiceTest
    {
       
        [TestMethod]
        public void backs_payment_when_balance_is_more_than_requested_test()
        {
            //Arrange
            MakePaymentRequest request = new MakePaymentRequest
            { 
                Amount = 200,
                CreditorAccountNumber = "SAVE1234",
                DebtorAccountNumber = "SAV1200",
                PaymentDate = DateTime.Parse("2022-06-19"),
                PaymentScheme = PaymentScheme.Bacs,
            };

            Account debtorAccount = new Account
            {
                AccountNumber = "SAVE1200",
                Balance = 300,
                Status = AccountStatus.Live,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
            };

            Mock<IAccountDataStore> accountDataStoreMock = new Mock<IAccountDataStore>();
            accountDataStoreMock.Setup(m => m.GetAccount(request.DebtorAccountNumber)).Returns(debtorAccount);

            Mock<BacsPaymentValidator> backsPaymentValidator = new Mock<BacsPaymentValidator>();
        

            Mock<IPaymentValidatorFactory> PaymentValidatorFactoryMock = new Mock<IPaymentValidatorFactory>();
            PaymentValidatorFactoryMock.Setup(m => m.GetPaymentValidator(request.PaymentScheme)).Returns(backsPaymentValidator.Object);

            IPaymentService paymentService = new PaymentService(accountDataStoreMock.Object, PaymentValidatorFactoryMock.Object);

            decimal expectedAccountBalance = debtorAccount.Balance - request.Amount;

            //Act
            var result =  paymentService.MakePayment(request);

            //Assert
            Assert.IsTrue(result.Success && expectedAccountBalance == debtorAccount.Balance);
        }
        [TestMethod]
        public void backs_Payment_when_balance_is_less_than_requested_test()
        {
            //Arrange
            MakePaymentRequest request = new MakePaymentRequest
            {
                Amount = 500,
                CreditorAccountNumber = "SAVE1234",
                DebtorAccountNumber = "SAV1200",
                PaymentDate = DateTime.Parse("2022-06-19"),
                PaymentScheme = PaymentScheme.Bacs,
            };

            Account debtorAccount = new Account
            {
                AccountNumber = "SAVE1200",
                Balance = 300,
                Status = AccountStatus.Live,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
            };


            Mock<IAccountDataStore> accountDataStoreMock = new Mock<IAccountDataStore>();
            accountDataStoreMock.Setup(m => m.GetAccount(request.DebtorAccountNumber)).Returns(debtorAccount);

            Mock<BacsPaymentValidator> backsPaymentValidator = new Mock<BacsPaymentValidator>();
            
            Mock<IPaymentValidatorFactory> PaymentValidatorFactoryMock = new Mock<IPaymentValidatorFactory>();
            PaymentValidatorFactoryMock.Setup(m => m.GetPaymentValidator(request.PaymentScheme)).Returns(backsPaymentValidator.Object);

            IPaymentService paymentService = new PaymentService(accountDataStoreMock.Object, PaymentValidatorFactoryMock.Object);


            //Act
            var result = paymentService.MakePayment(request);

            //Assert
            Assert.IsTrue(!result.Success);
        }

        [TestMethod]
        public void backs_payment_when_allowed_scheme_is_chaps_test()
        {
            //Arrange
            MakePaymentRequest request = new MakePaymentRequest
            {
                Amount = 500,
                CreditorAccountNumber = "SAVE1234",
                DebtorAccountNumber = "SAV1200",
                PaymentDate = DateTime.Parse("2022-06-19"),
                PaymentScheme = PaymentScheme.Bacs,
            };

            Account debtorAccount = new Account
            {
                AccountNumber = "SAVE1200",
                Balance = 300,
                Status = AccountStatus.Live,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps
            };


            Mock<IAccountDataStore> accountDataStoreMock = new Mock<IAccountDataStore>();
            accountDataStoreMock.Setup(m => m.GetAccount(request.DebtorAccountNumber)).Returns(debtorAccount);

            Mock<BacsPaymentValidator> backsPaymentValidator = new Mock<BacsPaymentValidator>();

            Mock<IPaymentValidatorFactory> PaymentValidatorFactoryMock = new Mock<IPaymentValidatorFactory>();
            PaymentValidatorFactoryMock.Setup(m => m.GetPaymentValidator(request.PaymentScheme)).Returns(backsPaymentValidator.Object);

            IPaymentService paymentService = new PaymentService(accountDataStoreMock.Object, PaymentValidatorFactoryMock.Object);


            //Act
            var result = paymentService.MakePayment(request);

            //Assert
            Assert.IsTrue(!result.Success);
        }

        [TestMethod]
        public void backs_payment_when_account_is_disabled_test()
        {
            //Arrange
            MakePaymentRequest request = new MakePaymentRequest
            {
                Amount = 500,
                CreditorAccountNumber = "SAVE1234",
                DebtorAccountNumber = "SAV1200",
                PaymentDate = DateTime.Parse("2022-06-19"),
                PaymentScheme = PaymentScheme.Bacs,
            };

            Account debtorAccount = new Account
            {
                AccountNumber = "SAVE1200",
                Balance = 300,
                Status = AccountStatus.Disabled,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps
            };


            Mock<IAccountDataStore> accountDataStoreMock = new Mock<IAccountDataStore>();
            accountDataStoreMock.Setup(m => m.GetAccount(request.DebtorAccountNumber)).Returns(debtorAccount);

            Mock<BacsPaymentValidator> backsPaymentValidator = new Mock<BacsPaymentValidator>();

            Mock<IPaymentValidatorFactory> PaymentValidatorFactoryMock = new Mock<IPaymentValidatorFactory>();
            PaymentValidatorFactoryMock.Setup(m => m.GetPaymentValidator(request.PaymentScheme)).Returns(backsPaymentValidator.Object);

            IPaymentService paymentService = new PaymentService(accountDataStoreMock.Object, PaymentValidatorFactoryMock.Object);


            //Act
            var result = paymentService.MakePayment(request);

            //Assert
            Assert.IsTrue(!result.Success);
        }

        [TestMethod]
        public void faster_payment_when_balance_is_more_than_requested_test()
        {
            //Arrange
            MakePaymentRequest request = new MakePaymentRequest
            {
                Amount = 200,
                CreditorAccountNumber = "SAVE1234",
                DebtorAccountNumber = "SAV1200",
                PaymentDate = DateTime.Parse("2022-06-19"),
                PaymentScheme = PaymentScheme.FasterPayments,
            };

            Account debtorAccount = new Account
            {
                AccountNumber = "SAVE1200",
                Balance = 300,
                Status = AccountStatus.Live,
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments
            };

            Mock<IAccountDataStore> accountDataStoreMock = new Mock<IAccountDataStore>();
            accountDataStoreMock.Setup(m => m.GetAccount(request.DebtorAccountNumber)).Returns(debtorAccount);

            Mock<FasterPaymentValidator> backsPaymentValidator = new Mock<FasterPaymentValidator>();


            Mock<IPaymentValidatorFactory> PaymentValidatorFactoryMock = new Mock<IPaymentValidatorFactory>();
            PaymentValidatorFactoryMock.Setup(m => m.GetPaymentValidator(request.PaymentScheme)).Returns(backsPaymentValidator.Object);

            IPaymentService paymentService = new PaymentService(accountDataStoreMock.Object, PaymentValidatorFactoryMock.Object);

            decimal expectedAccountBalance = debtorAccount.Balance - request.Amount;

            //Act
            var result = paymentService.MakePayment(request);

            //Assert
            Assert.IsTrue(result.Success && expectedAccountBalance == debtorAccount.Balance);
        }
    }
}
