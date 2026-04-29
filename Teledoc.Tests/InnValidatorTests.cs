using Microsoft.Extensions.Logging;
using Moq;
using Teledoc.ApiServices.Validators.Interfaces;
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
        public void IsCorrectINN()
        {
            var INN1 = "500100732259";
            var INN2 = "884932071575";

            var result = _validator.IsValidIndividualInn(INN1);
            var result2 = _validator.IsValidIndividualInn(INN2);


            Assert.True(result);
            Assert.True(result2);
        }

        public void IsNotCorrectINN()
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
    }
}
