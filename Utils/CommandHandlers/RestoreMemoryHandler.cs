using VitoBox.Constants;
using VitoBox.Models.Enums;
using VitoBox.Models.Interfaces;
using VitoBox.Models;
using VitoBox.Runtime;

namespace VitoBox.Utils.CommandHandlers;

public class RestoreMemoryHandler : IVitoCommandHandler
{
    public bool CanHandle(VitoCommandContext context)
    {
        if (VitoMemory.State != VitoState.Restoring)
            return false;

        var input = context.Prompt?.Trim();
        return input == Prophecy.SecretSpell || input == VitoPhrases.LostMemoryPrompt;
    }

    public Task HandleAsync(VitoCommandContext context, CancellationToken token)
    {
        VitoMemory.Restore();
        VitoMemory.ResetLostAttempts();

        context.Respond(new VitoResponse
        {
            Text = VitoPhrases.RestoreMemoryDisplay,
            Sound = VitoSounds.RestoreMemory,
            Vibration = true
        });

        Logger.LogInfo("Память Вито восстановлена.");
        return Task.CompletedTask;
    }
}

