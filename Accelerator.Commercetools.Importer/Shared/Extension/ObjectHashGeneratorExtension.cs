using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Accelerator.Commercetools.Importer.Shared.Extension;

public static class ObjectHashGeneratorExtension
{
    public static string GetObjectHashCode<T>(this T obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }
        
        var serialized = JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
        
        var bytes = Encoding.UTF8.GetBytes(serialized);
        using var sha512 = SHA512.Create();
        return Convert.ToHexString(sha512.ComputeHash(bytes));
    }
}