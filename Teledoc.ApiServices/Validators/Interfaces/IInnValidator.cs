namespace Teledoc.ApiServices.Validators.Interfaces
{


    public interface IInnValidator
    {
        bool IsValid(string inn);

        bool IsValidLegalEntityInn(string inn);

        bool IsValidIndividualInn(string inn);
    }
}
