using VirtualPet.Domain.Entities;
using VirtualPet.Domain.Enums;
using VirtualPet.Domain.Exceptions;

namespace VirtualPet.Domain.Tests.Entities
{
    public sealed class PetTests
    {
        [Fact]
        public void Constructor_WithValidData_ShouldCreatePetWithDefaultValues()
        {
            // Arrange 準備測試資料
            var userId = Guid.NewGuid();

            //Act 執行真正要測試的功能
            var pet = new Pet(userId, "Mochi", PetSpecies.Cat);

            //Assert 驗證結果
            Assert.NotEqual(Guid.Empty, pet.Id);
            Assert.Equal(userId, pet.UserId);
            Assert.Equal("Mochi", pet.Name);
            Assert.Equal(PetSpecies.Cat, pet.Species);

            Assert.Equal(1, pet.Level);
            Assert.Equal(0, pet.Experience);

            Assert.Equal(100, pet.Status.Satiety);
            Assert.Equal(100, pet.Status.Happiness);
            Assert.Equal(100, pet.Status.Energy);

            Assert.Equal(PetEvolutionStage.Egg, pet.EvolutionStage);
            Assert.Equal(PetState.Idle, pet.State);

        }

        [Fact]
        public void GainExperience_With250Experience_ShouldLevelUpTwice()
        {
            // Arrange
            var pet = new Pet(Guid.NewGuid(), "Mochi", PetSpecies.Cat);

            // Act
            pet.GainExperience(250);

            // Assert
            Assert.Equal(3, pet.Level);
            Assert.Equal(50, pet.Experience);
        }

        [Fact]
        public void GainExperience_WithZeroExperience_ShouldThrowException()
        {
            // Arrange
            var pet = new Pet(Guid.NewGuid(), "Mochi", PetSpecies.Cat);

            // Act
            var action = () => pet.GainExperience(0);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(action);
        }

        [Fact]
        public void ApplyTimeProgress_AfterTwoHours_ShouldDecreaseStatus()
        {
            // Arrange
            var pet = new Pet(Guid.NewGuid(), "Mochi", PetSpecies.Cat);

            var utcNow = pet.LastStatusUpdateAt.AddHours(2);

            // Act
            var changed = pet.ApplyTimeProgress(utcNow);

            // Assert
            Assert.True(changed);

            Assert.Equal(90, pet.Status.Satiety);
            Assert.Equal(96, pet.Status.Happiness);
            Assert.Equal(94, pet.Status.Energy);

            Assert.Equal(utcNow, pet.LastStatusUpdateAt);
        }


        public void Play_WhenEnergyIsTooLow_ShouldThrowException()
        {
            // Arrange
            var pet = new Pet(Guid.NewGuid(), "Mochi", PetSpecies.Cat);

            pet.ApplyTimeProgress(pet.LastStatusUpdateAt.AddHours(31));

            //Act
            var action = ()=> pet.Play();

            //Assert
            Assert.Throws<PetActionNotAllowedException>(action);
        }
    }
}
