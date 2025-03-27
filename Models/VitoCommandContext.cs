using VitoBox.Constants;
using VitoBox.Models.Enums;
using VitoBox.Models.Interfaces;

namespace VitoBox.Models;

public class VitoCommandContext
{
    public required VitoCommandType Type { get; init; }
    public string? Prompt { get; init; }

    public required ISerialPort Serial { get; init; }
    public required IVitoApiService Api { get; init; }

    public void Respond(VitoResponse response)
    {
        Serial.WriteLine($"{VitoConstants.OutDisplay}{response.Text}");

        if (!string.IsNullOrEmpty(response.Sound))
            Serial.WriteLine($"{VitoConstants.OutSound}{response.Sound}");

        if (response.Vibration)
            Serial.WriteLine(VitoConstants.OutVibrate);
    }

    public void ShowGlitch(string? glitchText = null, string? sound = null, bool vibration = false)
    {
        glitchText ??= Random.Shared.NextDouble() < 0.5
                        ? "GLITCH//ACTIVE"
                        : "@@# // ⍯⊠ [0xB1]";

        Serial.WriteLine($"{VitoConstants.OutDisplay}{glitchText}");
        Serial.WriteLine($"{VitoConstants.OutSound}{VitoSounds.GlitchBuzz}");
    }
}
