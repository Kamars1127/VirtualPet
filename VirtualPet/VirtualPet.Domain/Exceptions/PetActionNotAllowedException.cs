namespace VirtualPet.Domain.Exceptions
{
    /// <summary>
    /// Pet 無法執行指定行為時所產生的例外
    /// </summary>
    public class PetActionNotAllowedException : DomainException
    {
        public PetActionNotAllowedException(string message) : base(message)
        {
        }
    }
}
