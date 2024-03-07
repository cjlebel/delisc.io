using System.Security.Cryptography;
using System.Text;

namespace Deliscio.Common.Helpers;

public class GuidHelpers
{
    /// <summary>
    /// Creates a unique id as a (string) GUID based on the username that was provided.
    /// </summary>
    /// <param name="input">The name of the user to create the string for.</param>
    /// <returns>A GUID as a string</returns>
    public static Guid GetMD5AsGuid(string input)
    {
        // Convert the input string to a byte array and compute the hash.
        byte[] data = MD5.HashData(Encoding.Default.GetBytes(input));

        return new Guid(data);
    }
}