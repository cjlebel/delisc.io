using Ardalis.GuardClauses;
using Habanerio.Core.BuildingBlocks.Domain;

namespace Deliscio.Modules.Links.Domain.Links;
public sealed class LinkDomain : ValueObject
{
    public string Value { get; }

    private LinkDomain()
    {
        Value = string.Empty;
    }


    public LinkDomain(string domain)
    {
        var newDomain = domain?.Trim();

        Guard.Against.NullOrEmpty(newDomain, nameof(domain));

        Value = newDomain;
    }

    public static LinkDomain FromUrl(string url)
    {
        var newUrl = url?.Trim();

        Guard.Against.NullOrEmpty(newUrl, nameof(url));

        var commonSubdomains = new List<string>
        {
            "www",
            "mail",
            "ftp"
        };

        if (Uri.TryCreate(newUrl, UriKind.Absolute, out Uri? uri))
        {
            string host = uri.Host.ToLower();

            var matchingSubdomain = commonSubdomains.Find(subdomain => host.StartsWith(subdomain + "."));
            if (!string.IsNullOrWhiteSpace(matchingSubdomain))
            {
                // Remove the subdomain and the following dot.
                // This will leave intact any domain which has a custom subdomain (e.g. "mycustomsubdomain.example.com")
                host = host.Substring(matchingSubdomain.Length + 1);
            }

            return new LinkDomain(host);
        }

        return new LinkDomain();
    }
}
