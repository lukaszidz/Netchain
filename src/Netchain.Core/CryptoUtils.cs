using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Netchain.Core;

public static class CryptoUtils
{
    public static bool IsValidProof(int lastProof, int proof, string previousHash)
    {
        string guess = $"{lastProof}{proof}{previousHash}";
        string result = GetSha256(guess);
        return result.StartsWith("00");
    }

    public static string GetHash(Block block) => GetSha256(JsonSerializer.Serialize(new { block.Index, block.Timestamp, block.PreviousHash, block.Proof }));

    public static string GetSha256(string data)
    {
        var hashBuilder = new StringBuilder();

        byte[] bytes = Encoding.Unicode.GetBytes(data);
        byte[] hash = SHA256.HashData(bytes);

        foreach (var x in hash)
            hashBuilder.Append($"{x:x2}");

        return hashBuilder.ToString();
    }
}
