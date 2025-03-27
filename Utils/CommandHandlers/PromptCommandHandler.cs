using VitoBox.Models.Interfaces;
using VitoBox.Models;
using VitoBox.Constants;
using VitoBox.Runtime;

namespace VitoBox.Utils.CommandHandlers;

public class PromptCommandHandler : IVitoCommandHandler
{
    public bool CanHandle(VitoCommandContext context)
    {
        if (VitoMemory.IsGlitched && GlitchEscapeAttemptHandler.MatchesPrompt(context.Prompt))
            return false;

        return context.Prompt is not null;
    }

    public async Task HandleAsync(VitoCommandContext context, CancellationToken token)
    {
        var vito = await context.Api.GetVitoResponseAsync(context.Prompt!, token);

        if (VitoMemory.IsGlitched)
        {
            vito.Text = GlitchMutator.Mutate(vito.Text);
            vito.Sound ??= VitoSounds.GlitchBuzz;
        }
        else if (VitoMemory.IsReady)
        {
            VitoMemory.ReturnToreality();

            var line = VitoPhrases.ReturnedLines[Random.Shared.Next(VitoPhrases.ReturnedLines.Length)];
            context.Respond(new VitoResponse
            {
                Text = line,
                Sound = VitoSounds.RestoreMemory,
                Vibration = true,
            });
            return;
        }

        context.Respond(vito);
        Logger.LogInfo($"Отправлено: {vito.Text}");
    }
}
