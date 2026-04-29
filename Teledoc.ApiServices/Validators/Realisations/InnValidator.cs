using Teledoc.ApiServices.Validators.Interfaces;

namespace Teledoc.ApiServices.Validators.Realisations
{

    public sealed class InnValidator : IInnValidator
    {
        public bool IsValidIndividualInn(string inn)
        {
            if (string.IsNullOrWhiteSpace(inn))
                return false;

            inn = inn.Trim();
            if (inn.Distinct().Count() == 1)
                return false;
            if (inn.Length != 12)
                return false;

            if (!inn.All(char.IsDigit))
                return false;

            var digits = inn.Select(c => c - '0').ToArray();

            int[] firstControlCoefficients = { 7, 2, 4, 10, 3, 5, 9, 4, 6, 8 };
            int[] secondControlCoefficients = { 3, 7, 2, 4, 10, 3, 5, 9, 4, 6, 8 };

            var firstControlNumber = CalculateControlNumber(digits, firstControlCoefficients);
            var secondControlNumber = CalculateControlNumber(digits, secondControlCoefficients);

            return firstControlNumber == digits[10]
                && secondControlNumber == digits[11];
        }

        private int CalculateControlNumber(int[] digits, int[] coefficients)
        {
            var sum = coefficients
                .Select((coefficient, index) => coefficient * digits[index])
                .Sum();

            return sum % 11 % 10;
        }
    }
}
