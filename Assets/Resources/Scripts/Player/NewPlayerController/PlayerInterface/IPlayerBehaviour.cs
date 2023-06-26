namespace NewPlayerController
{
    public interface IPlayerBehaviour
    {
        IPlayerBehaviourData PlayerData { get; }

        void EnterBehaviour();
        void UpdateBehaviour();
        void ExitBehaviour();
        void SetNewBehaviour<T>() where T : IPlayerBehaviour;
    }
}
