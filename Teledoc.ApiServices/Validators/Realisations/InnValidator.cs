using Teledoc.ApiServices.Validators.Interfaces;

namespace Teledoc.ApiServices.Validators.Realisations
{

    public sealed class InnValidator : IInnValidator
    {
        public bool IsValid(string inn)
        {
            if (string.IsNullOrWhiteSpace(inn))
                return false;

            inn = inn.Trim();

            return inn.Length switch
            {
                10 => IsValidLegalEntityInn(inn),
                12 => IsValidIndividualInn(inn),
                _ => false
            };
        }

        public bool IsValidLegalEntityInn(string inn)
        {
            if (!HasValidFormat(inn, 10))
                return false;

            var digits = ToDigits(inn);

            int[] coefficients = { 2, 4, 10, 3, 5, 9, 4, 6, 8 };

            var controlNumber = CalculateControlNumber(digits, coefficients);

            return controlNumber == digits[9];
        }

        public bool IsValidIndividualInn(string inn)
        {
            if (!HasValidFormat(inn, 12))
                return false;

            var digits = ToDigits(inn);

            int[] firstControlCoefficients = { 7, 2, 4, 10, 3, 5, 9, 4, 6, 8 };
            int[] secondControlCoefficients = { 3, 7, 2, 4, 10, 3, 5, 9, 4, 6, 8 };

            var firstControlNumber = CalculateControlNumber(digits, firstControlCoefficients);
            var secondControlNumber = CalculateControlNumber(digits, secondControlCoefficients);

            return firstControlNumber == digits[10]
                && secondControlNumber == digits[11];
        }

        private bool HasValidFormat(string inn, int expectedLength)
        {
            if (inn.Length != expectedLength)
                return false;

            if (!inn.All(char.IsDigit))
                return false;

            if (inn.Distinct().Count() == 1)
                return false;

            return true;
        }

        private int[] ToDigits(string inn)
        {
            return inn.Select(c => c - '0').ToArray();
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
