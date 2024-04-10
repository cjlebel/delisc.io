namespace Deliscio.Modules.Authentication.Common.ValueObjects;

public record Email
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        return new Email(value);
    }
}