using System.Security.Cryptography;
using System.Text;

namespace VitoBox.Security;

public static class VitoSeal
{
    private const string SealFile = "vitoseal.sig";
    private const string HiddenTruth = "По коням.";

    public static bool IsSealValid()
    {
        if (!File.Exists(SealFile))
            return false;

        var content = File.ReadAllText(SealFile).Trim();
        var expected = ComputeHash(HiddenTruth);
        return content == expected;
    }

    public static void WriteSeal()
    {
        var hash = ComputeHash(HiddenTruth);
        File.WriteAllText(SealFile, hash);
    }

    private static string ComputeHash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = sha.ComputeHash(bytes);
        return Convert.ToHexString(hashBytes);
    }
}
