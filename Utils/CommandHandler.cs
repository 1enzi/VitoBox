using VitoBox.Constants;
using VitoBox.Models.Enums;
using VitoBox.Runtime;

namespace VitoBox.Utils;

public class CommandHandler
{
    private static readonly Dictionary<VitoButtonCommand, (VitoCommandType, string?)> CommandMap = new()
    {
        [VitoButtonCommand.Compliment] = (VitoCommandType.Compliment, VitoPhrases.PromptCompliment),
        [VitoButtonCommand.Meme] = (VitoCommandType.Meme, VitoPhrases.PromptMeme),
        [VitoButtonCommand.Quote] = (VitoCommandType.Quote, VitoPhrases.PromptQuote),
        [VitoButtonCommand.Debug] = (VitoCommandType.Debug, VitoPhrases.PromptDebug),
        [VitoButtonCommand.KillVito] = (VitoCommandType.Shutdown, null),
        [VitoButtonCommand.InvokeGlitch] = (VitoCommandType.Glitch, VitoPhrases.PromptGlitch),
    };

    private static readonly Dictionary<VitoButtonCommand, Func<(VitoCommandType, string?)>> SpecialHandlers = new()
    {
        [VitoButtonCommand.RestoreMemory] = () =>
        {
            VitoMemory.BeginRestoration();
            return (VitoCommandType.Debug, VitoPhrases.LostMemoryPrompt);
        },
        [VitoButtonCommand.ArchiveWhisper] = () =>
        {
            VitoMemory.TriggerArchive();
            return (VitoCommandType.Debug, VitoPhrases.ArchiveTriggerPrompt);
        },
    };

    public (VitoCommandType Type, string? Prompt) Parse(string? raw)
    {
        (var type, var prompt) = CommandParser.Preprocess(raw);

        if (type != VitoCommandType.Unknown)
            return (type, prompt);

        return ParsePreprocessed(type, prompt);
    }


    private (VitoCommandType, string?) ParsePreprocessed(VitoCommandType type, string? prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt))
            return (VitoCommandType.Unknown, null);

        if (!VitoButtonMap.Map.TryGetValue(prompt, out var button))
            return (VitoCommandType.Unknown, null);

        if (SpecialHandlers.TryGetValue(button, out var handler))
            return handler();

        return CommandMap.TryGetValue(button, out var pair)
            ? pair
            : (VitoCommandType.Unknown, null);
    }
}


