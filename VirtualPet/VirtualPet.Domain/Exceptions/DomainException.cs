namespace VirtualPet.Domain.Exceptions
{
    /// <summary>
    /// Domain 規則違反時所產生的基礎例外
    /// </summary>
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {

        }
    }
}
