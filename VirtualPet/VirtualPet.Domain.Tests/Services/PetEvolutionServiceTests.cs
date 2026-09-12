using VirtualPet.Domain.Entities;
using VirtualPet.Domain.Enums;
using VirtualPet.Domain.Services;

namespace VirtualPet.Domain.Tests.Services
{
    public sealed class PetEvolutionServiceTests
    {
        [Fact]
        public void TryEvolve_WhenPetMeetsEggRule_ShouldEvolveToBaby()
        {
            // Arrange
            var pet = new Pet(Guid.NewGuid(), "Mochi", PetSpecies.Cat);

            pet.GainExperience(100);

            var service = new PetEvolutionService();

            // Act
            var evolved = service.TryEvolve(pet);

            // Assert
            Assert.True(evolved);
            Assert.Equal(PetEvolutionStage.Baby, pet.EvolutionStage);
        }

        [Fact]
        public void TryEvolve_WhenPetDoesNotMeetRule_ShouldNotEvolve()
        {
            // Arrange
            var pet = new Pet(Guid.NewGuid(), "Mochi", PetSpecies.Cat);
            var service = new PetEvolutionService();

            //Act
            var evolved = service.TryEvolve(pet);

            // Assert
            Assert.False(evolved);
            Assert.Equal(PetEvolutionStage.Egg, pet.EvolutionStage);
        }
    }
}
