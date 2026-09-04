using VirtualPet.Domain.Enums;
using VirtualPet.Domain.Exceptions;
using VirtualPet.Domain.ValueObjects;

namespace VirtualPet.Domain.Entities
{
    /// <summary>
    /// 寵物領域實體
    /// </summary>
    public class Pet
    {
        private readonly List<PetHistory> _histories = [];

        /// <summary>
        /// 唯一識別碼
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// 所屬玩家 Id
        /// </summary>
        public Guid UserId { get; private set; }

        public User User { get; private set; }

        #region /*--- State ---*/
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 種類
        /// </summary>
        public PetSpecies Species{ get; private set; }

        /// <summary>
        /// 進化階段
        /// </summary>
        public PetEvolutionStage EvolutionStage { get; private set; }

        /// <summary>
        /// 目前狀態
        /// </summary>
        public PetState State { get; private set; }

        /// <summary>
        /// 等級
        /// </summary>
        public int Level {  get; private set; }

        /// <summary>
        /// 經驗值
        /// </summary>
        public int Experience {  get; private set; }

        /// <summary>
        /// 狀態數值
        /// </summary>
        public PetStatusValue Status { get; private set; }
        
        

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateAt { get; private set; }
        
        #endregion

        /// <summary>
        /// Pet 的操作紀錄
        /// </summary>
        public IReadOnlyCollection<PetHistory> Histories => _histories.AsReadOnly();

        private Pet()
        {
            Name = string.Empty;
        }

        public Pet(Guid userId, string name, PetSpecies species)
        {
            if(userId == Guid.Empty)
            {
                throw new ArgumentException("User id cannot be empty.", nameof(userId));
            }

            //寵物名稱不能為空
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Pet name cannot be empty.", nameof(name));
            }

            //寵物名稱最多20個字元
            if(name.Length > 20)
            {
                throw new ArgumentException("Pet name cannot be longer than 20 characters.", nameof(name));
            }

            Id = Guid.NewGuid();
            Name = name;
            Species = species;
            Level = 1;
            Experience = 0;
            EvolutionStage = PetEvolutionStage.Egg;

            State = PetState.Idle;

            Status = new PetStatusValue(
                satiety: 0,
                happiness: 0,
                energy: 0);

            
            CreateAt = DateTime.UtcNow;
        }


        /// <summary>
        /// 餵食
        /// </summary>
        public void Feed()
        {
            State = PetState.Eating;

            Status = Status.Change(satiety: 20);

            State = PetState.Idle;
        }

        /// <summary>
        /// 玩
        /// </summary>
        public void Play()
        {
            if (Status.Energy < 10)
            {
                throw new PetActionNotAllowedException("Pet does not have enough energy to play.");
            }

            State = PetState.Playing;

            Status = Status.Change(satiety: -5, happiness: 20, energy: -10);

            State = PetState.Idle;
        }

        /// <summary>
        /// 休息
        /// </summary>
        public void Rest()
        {
            State = PetState.Sleeping;

            Status = Status.Change(energy: 30);

            State = PetState.Idle;
        }

        /// <summary>
        /// 增加經驗值
        /// </summary>
        /// <param name="exp">增加的經驗值</param>
        public void GainExperience(int exp)
        {
            if (exp <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(exp),
                    "Experience amount must be greater than zero.");
            }

            Experience += exp;

            while (Experience >= 100)
            {
                LevelUp();
            }
        }

        /// <summary>
        /// 升級
        /// </summary>
        private void LevelUp()
        {
            Level++;
            Experience -= 100;
        }

        /// <summary>
        /// 進化至下一個成長階段
        /// </summary> 
        internal void EvolveTo(PetEvolutionStage nextStage)
        {
            PetEvolutionStage? expectedNextStage = EvolutionStage switch
            {
                PetEvolutionStage.Egg => PetEvolutionStage.Baby,
                PetEvolutionStage.Baby => PetEvolutionStage.Child,
                PetEvolutionStage.Child => PetEvolutionStage.Adult,
                PetEvolutionStage.Adult => null,
                _ => null
            };

            if (expectedNextStage is null)
            {
                throw new PetEvolutionException("Pet cannot evolve any further.");
            }

            if (nextStage != expectedNextStage.Value)
            {
                throw new PetEvolutionException($"Pet cannot evolve from {EvolutionStage} to {nextStage}.");
            }

            EvolutionStage = nextStage;
        }
    }
}
