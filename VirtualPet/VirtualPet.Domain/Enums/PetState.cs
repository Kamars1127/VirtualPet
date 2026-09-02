namespace VirtualPet.Domain.Enums
{
    /// <summary>
    /// 寵物目前狀態
    /// </summary>
    public enum PetState
    {
        /// <summary>
        /// 閒置
        /// </summary>
        Idle = 1,
        /// <summary>
        /// 進食中
        /// </summary>
        Eating = 2,
        /// <summary>
        /// 睡眠中
        /// </summary>
        Sleeping = 3,
        /// <summary>
        /// 玩耍中
        /// </summary>
        Playing = 4
    }
}
