namespace Deliscio.Core.Models;


public class ResultsBase
{
    public bool IsError { get; init; }

    public bool IsSuccess => !IsError;

    public string Message { get; init; } = string.Empty;

    protected ResultsBase() { }
}

public class Results : ResultsBase
{
    internal Results() { }

    public static Results Fail(string message)
    {
        return new Results { IsError = true, Message = message };
    }

    public static Results Fail(Exception ex)
    {
        return new Results { IsError = true, Message = ex.Message };
    }

    public static Results Ok()
    {
        return new Results { IsError = false };
    }
}

public class Results<T> : ResultsBase
{
    public T? Item { get; init; }

    internal Results() { }

    public new static Results<T?> Fail(string message)
    {
        return new Results<T?> { IsError = true, Message = message };
    }

    public new static Results<T> Fail(Exception ex)
    {
        return new Results<T> { IsError = true, Message = ex.Message };
    }

    public static Results<T> Ok(T item)
    {
        return new Results<T> { Item = item, IsError = false };
    }
}