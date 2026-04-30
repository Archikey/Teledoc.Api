using Teledoc.ApiServices.Validators.Realisations;

namespace Teledoc.Tests
{
    public class InnValidatorTests
    {
        private readonly InnValidator _validator = new();
        public InnValidatorTests()
        {
            
            
        }


        [Fact]
        public void IsCorrectINNForIndividuals()
        {
            var INN1 = "500100732259";
            var INN2 = "884932071575";

            var result = _validator.IsValidIndividualInn(INN1);
            var result2 = _validator.IsValidIndividualInn(INN2);


            Assert.True(result);
            Assert.True(result2);
        }
        [Fact]
        public void IsNotCorrectINNForIndividuals()
        {
            var INN1 = "111111111111";
            var INN2 = "111111111212";
            var INN3 = "000000000000";
            var INN4 = "";
            var INN5 = "12345678";

            var result = _validator.IsValidIndividualInn(INN1);
            var result2 = _validator.IsValidIndividualInn(INN2);
            var result3 = _validator.IsValidIndividualInn(INN3);
            var result4 = _validator.IsValidIndividualInn(INN4);
            var result5 = _validator.IsValidIndividualInn(INN5);


            Assert.False(result);
            Assert.False(result2);
            Assert.False(result3);
            Assert.False(result4);
            Assert.False(result5);
        }

        [Fact]
        public void IsCorrectINNForLegalEntities()
        {
            var INN1 = "5005247018";
            var INN2 = "5927654355";
            var INN3 = "0999316317";

            var result = _validator.IsValidLegalEntityInn(INN1);
            var result2 = _validator.IsValidLegalEntityInn(INN2);
            var result3 = _validator.IsValidLegalEntityInn(INN3);

            Assert.True(result);
            Assert.True(result2);
            Assert.True(result3);
        }

        [Fact]
        public void IsNotCorrectINNForLegalEntities()
        {
            var INN1 = "1111111111";
            var INN2 = "1111111112";
            var INN3 = "0000000000";
            var INN4 = "";
            var INN5 = "12345678";
            var INN6 = "5005247019";

            var result = _validator.IsValidLegalEntityInn(INN1);
            var result2 = _validator.IsValidLegalEntityInn(INN2);
            var result3 = _validator.IsValidLegalEntityInn(INN3);
            var result4 = _validator.IsValidLegalEntityInn(INN4);
            var result5 = _validator.IsValidLegalEntityInn(INN5);
            var result6 = _validator.IsValidLegalEntityInn(INN6);

            Assert.False(result);
            Assert.False(result2);
            Assert.False(result3);
            Assert.False(result4);
            Assert.False(result5);
            Assert.False(result6);
        }
    }
}
