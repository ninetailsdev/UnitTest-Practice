using System;
using Moq;
using Xunit;

namespace CreditCardApplications.Tests
{
    public class CreditCardApplicationEvaluatorShould
    {
        [Fact]
        public void AcceptHighIncomeApplications()
        {

            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication { GrossAnnualIncome = 100_000 };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.AutoAccepted, decision);
        }

        [Fact]
        public void ReferYoungApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication { Age = 19 };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        [Fact]
        public void DeclineLowIncomeApplications()
        {
            // set up specific attributes in mock object example
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            // pass string variable to target parameter
            // mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            //  mockValidator.Setup(x => x.IsValid(It.Is<string>(number => number.StartsWith('x')))).Returns(true);
            //mockValidator.Setup(x => x.IsValid(It.IsIn("x", "y", "z"))).Returns(true);
            //  mockValidator.Setup(x => x.IsValid(It.IsInRange("a", "z", Range.Inclusive))).Returns(true);
            

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var applicaation = new CreditCardApplication
            {
                GrossAnnualIncome = 19_999,
                Age= 42,
                FrequentFlyerNumber = "x"
            };

            var decision = sut.Evaluate(applicaation);
            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);
        }

        [Fact]
        public void ReferInvalidFrequentFlyerApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication();
            
            
            var decision = sut.Evaluate(application);
            
            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        [Fact]
        public void ReferWhenLicenceExpired()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            mockValidator.Setup(x => x.ServiceInformation.Licence.LicenceKey).Returns(GetLiceneceKeyExpired);
       

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication {Age = 42};


            var decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        [Fact]
        public void YoungFolks_Idunno()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            mockValidator.Setup(x => x.ServiceInformation.Licence.LicenceKey).Returns("OK");


            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication { Age = 19 };


            var decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        string GetLiceneceKeyExpired()
        {
            return "EXPIRED";
        }

        [Fact]
        public void EnumCheck()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            
            mockValidator.Setup(x => x.ServiceInformation.Licence.LicenceKey).Returns("OK");
            mockValidator.SetupProperty(x => x.ValidationMode);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication { Age = 30 };


            var decision = sut.Evaluate(application);

            Assert.Equal(ValidationMode.Detailed, mockValidator.Object.ValidationMode);
        }

        //behaviour tests
        [Fact]
        public void WasMEthodCalled()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.Licence.LicenceKey).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication();


            sut.Evaluate(application);

            mockValidator.Verify( x=>x.IsValid(null));
        }

        [Fact]
        public void CustomeOutputTxt()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.Licence.LicenceKey).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication();


            sut.Evaluate(application);

            mockValidator.Verify(x => x.IsValid(It.IsNotNull<string>()), "this took a 1 minute video to explain....");
        }

        [Fact]
        public void ErrorsFromMoq()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.Licence.LicenceKey).Returns("OK");
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication {Age = 42};


            sut.Evaluate(application);
            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }
    }
}
