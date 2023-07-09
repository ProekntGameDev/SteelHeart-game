using System;

public abstract class Interactor<T> where T : Repository, new()
{
    public virtual Repository Repository { get; private set; }
    protected virtual T Data { get; private set; }

    public Interactor()
    {
        Data = new T();
        Repository = Data;
    }
}
