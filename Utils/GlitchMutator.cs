namespace VitoBox.Utils;

public static class GlitchMutator
{
    private static readonly string[] GlitchFragments =
    {
        "⍯", "⊠", "⧖", "¿", "※", "@", "#", "░", "█", "⋘⋙"
    };

    private static readonly Dictionary<char, string> FlipMap = new()
    {
        ['a'] = "ɐ", ['b'] = "q", ['c'] = "ɔ", ['d'] = "p", ['e'] = "ǝ",
        ['f'] = "ɟ", ['g'] = "ƃ", ['h'] = "ɥ", ['i'] = "ᴉ", ['j'] = "ɾ",
        ['k'] = "ʞ", ['l'] = "ʃ", ['m'] = "ɯ", ['n'] = "u", ['o'] = "o",
        ['p'] = "d", ['q'] = "b", ['r'] = "ɹ", ['s'] = "s", ['t'] = "ʇ",
        ['u'] = "n", ['v'] = "ʌ", ['w'] = "ʍ", ['x'] = "x", ['y'] = "ʎ",
        ['z'] = "z",

        ['A'] = "∀", ['B'] = "ᗺ", ['C'] = "Ɔ", ['D'] = "◖", ['E'] = "Ǝ",
        ['F'] = "Ⅎ", ['G'] = "פ", ['H'] = "H", ['I'] = "I", ['J'] = "ſ",
        ['K'] = "ʞ", ['L'] = "˥", ['M'] = "W", ['N'] = "N", ['O'] = "O",
        ['P'] = "Ԁ", ['Q'] = "Ό", ['R'] = "ᴚ", ['S'] = "S", ['T'] = "┴",
        ['U'] = "∩", ['V'] = "Λ", ['W'] = "M", ['X'] = "X", ['Y'] = "⅄",
        ['Z'] = "Z",

        ['1'] = "Ɩ", ['2'] = "ᄅ", ['3'] = "Ɛ", ['4'] = "ㄣ", ['5'] = "ϛ",
        ['6'] = "9",  ['7'] = "ㄥ", ['8'] = "8", ['9'] = "6", ['0'] = "0",

        ['.'] = "˙", [','] = "'", ['\''] = ",", ['"'] = "„", ['!'] = "¡",
        ['?'] = "¿", ['('] = ")", [')'] = "(", ['['] = "]", [']'] = "[",
        ['{'] = "}", ['}'] = "{", ['<'] = ">", ['>'] = "<",
    };


    private static DateTime? _glitchStart;

    public static string Mutate(string input)
    {
        _glitchStart ??= DateTime.UtcNow;

        var glitchDuration = DateTime.UtcNow - _glitchStart.Value;
        var seconds = glitchDuration.TotalSeconds;

        if (seconds >= 60)
            return ReverseAndCorrupt(input);
        if (seconds >= 30)
            return FragmentedEcho(input);
        if (seconds >= 90)
            return DoubleCorruptedEcho(input);


        return RandomCorrupt(input, seconds);
    }

    private static string ReverseAndCorrupt(string input)
    {
        var reversed = new string(input.Reverse().ToArray());
        return $"⧖ {Corrupt(reversed)}";
    }

    private static string FragmentedEcho(string input)
    {
        var fragments = input.Split(' ')
                             .Select(w => Random.Shared.NextDouble() < 0.4 ? "…" : w)
                             .ToArray();

        return string.Join(" ", fragments);
    }

    private static string Corrupt(string input)
    {
        var chars = input.ToCharArray();
        var count = Math.Max(1, input.Length / 6);

        for (int i = 0; i < count; i++)
        {
            var index = Random.Shared.Next(chars.Length);
            chars[index] = GlitchFragments[Random.Shared.Next(GlitchFragments.Length)][0];
        }

        return new string(chars);
    }

    private static string RandomCorrupt(string input, double seconds)
    {
        var chars = input.ToCharArray();
        var glitchCount = Math.Max(1, (int)(input.Length * (seconds / 30.0) * 0.5));

        for (int i = 0; i < glitchCount; i++)
        {
            var index = Random.Shared.Next(chars.Length);
            chars[index] = GlitchFragments[Random.Shared.Next(GlitchFragments.Length)][0];
        }

        return new string(chars);
    }

    private static string DoubleCorruptedEcho(string input)
    {
        var flipped = FlipText(input);
        var corrupted = Corrupt(flipped);
        var pulseLevel = GetPulseLevel();
        var pulse = GetPulse(pulseLevel);

        return $"{input}\n{pulse}\n⧖ {corrupted}";
    }

    private static string FlipText(string input)
    {
        var flipped = input
            .Reverse()
            .Select(c => FlipMap.TryGetValue(c, out var flippedChar) ? flippedChar : c.ToString());

        return string.Concat(flipped);
    }

    private static string GetPulse(int level)
    {
        return string.Join(" ", Enumerable.Repeat("•", level));
    }

    private static int GetPulseLevel()
    {
        if (_glitchStart is null)
            return 1;

        var seconds = (DateTime.UtcNow - _glitchStart.Value).TotalSeconds;

        if (seconds >= 180) return 5;
        if (seconds >= 150) return 4;
        if (seconds >= 120) return 3;
        if (seconds >= 90) return 2;

        return 1;
    }

    public static void Reset()
    {
        _glitchStart = null;
    }
}

