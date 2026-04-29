namespace Teledoc.ApiServices.Validators.Interfaces
{

    /// <summary>
    /// Интерфейс валидатора
    /// </summary>
    public interface IInnValidator
    {
        /// <summary>
        /// Метод для валидации ИНН физических лиц
        /// </summary>
        /// <param name="inn">Номер ИНН</param>
        /// <returns>Возращет true или false</returns>
        bool IsValidIndividualInn(string inn);
    }
}
