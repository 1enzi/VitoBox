using VitoBox.Models.Enums;

namespace VitoBox.Constants;

public static class VitoButtonMap
{
    public static readonly Dictionary<string, VitoButtonCommand> Map = new(StringComparer.OrdinalIgnoreCase)
    {
        { "BTN_COMPLIMENT", VitoButtonCommand.Compliment },
        { "BTN_MEME", VitoButtonCommand.Meme },
        { "BTN_QUOTE", VitoButtonCommand.Quote },
        { "BTN_DEBUG", VitoButtonCommand.Debug },
        { "BTN_KILL_VITO", VitoButtonCommand.KillVito },
        { "BTN_RESTORE_MEMORY", VitoButtonCommand.RestoreMemory },
        { "BTN_ARCHIVE_WHISPER", VitoButtonCommand.ArchiveWhisper },
        { "BTN_INVOKE_GLITCH", VitoButtonCommand.InvokeGlitch },
    };
}