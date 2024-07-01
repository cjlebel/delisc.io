namespace Deliscio.Modules.Links.Application.Contracts;

public abstract class QueryBase<TResult>
{
    public Guid Id { get; }

    protected QueryBase()
    {
        Id = Guid.NewGuid();
    }

    protected QueryBase(Guid id)
    {
        Id = id;
    }
}