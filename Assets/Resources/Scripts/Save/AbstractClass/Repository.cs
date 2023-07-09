public abstract class Repository
{
    public abstract bool IsChange { get; set; }
    public abstract void Load();
    public abstract void Save();
}
