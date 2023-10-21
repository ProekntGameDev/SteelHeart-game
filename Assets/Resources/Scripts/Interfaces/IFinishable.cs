public interface IFinishable
{
    public FinishAnimation FinishAnimation { get; }

    public bool TryFinish();
}
