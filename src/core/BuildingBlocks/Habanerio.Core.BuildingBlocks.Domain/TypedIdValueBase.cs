using System.Diagnostics.CodeAnalysis;

namespace Habanerio.Core.BuildingBlocks.Domain;

public abstract record TypedIdValueBase<TType>(TType value) : IEqualityComparer<TypedIdValueBase<TType>>
{
    public TType Value { get; } = value;

    public override string ToString() => Value?.ToString() ?? string.Empty; // throw new InvalidOperationException(nameof(Value));


    public virtual bool Equals(TypedIdValueBase<TType>? other)
    {
        if (other is null)
            return false;

        if (other.Value is null)
            return false;

        return other.Value.Equals(Value);
    }

    public bool Equals(TypedIdValueBase<TType>? x, TypedIdValueBase<TType>? y)
    {
        if (object.Equals(x, null))
        {
            if (object.Equals(y, null))
            {
                return true;
            }

            return false;
        }

        return x.Equals(y);
    }

    public override int GetHashCode()
    {
        return Value?.GetHashCode() ?? throw new InvalidOperationException(nameof(Value));
    }

    public int GetHashCode([DisallowNull] TypedIdValueBase<TType> obj)
    {
        return Value?.GetHashCode() ?? throw new InvalidOperationException(nameof(Value));
    }
}