namespace NewPlayerController
{
    public interface IPlayerBehaviour
    {
        bool IsActive { get; }
        IPlayerBehaviourData PlayerData { get; }

        void EnterBehaviour();
        void UpdateBehaviour();
        void ExitBehaviour();
    }
}
