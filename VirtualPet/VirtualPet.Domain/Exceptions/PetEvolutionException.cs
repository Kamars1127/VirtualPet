namespace VirtualPet.Domain.Exceptions
{
    /// <summary>
    /// Pet 進化規則被違反時所產生的例外
    /// </summary>
    public class PetEvolutionException : DomainException
    {
        public PetEvolutionException(string message) : base(message)
        {
        }
    }
}
