namespace Deliscio.Modules.Links.Application.Contracts;

public abstract class LinksCommandBase : ILinksCommand
{
    public Guid Id { get; }

    protected LinksCommandBase()
    {
        Id = Guid.NewGuid();
    }

    protected LinksCommandBase(Guid id)
    {
        Id = id;
    }
}

public abstract class LinksCommandBase<TResult> : ILinksCommand<TResult>
{
    protected LinksCommandBase()
    {
        Id = Guid.NewGuid();
    }

    protected LinksCommandBase(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}