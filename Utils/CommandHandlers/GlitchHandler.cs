using VitoBox.Constants;
using VitoBox.Models.Enums;
using VitoBox.Models.Interfaces;
using VitoBox.Models;
using VitoBox.Runtime;

namespace VitoBox.Utils.CommandHandlers;

public class GlitchCommandHandler : IVitoCommandHandler
{
    public bool CanHandle(VitoCommandContext context) =>
        context.Type == VitoCommandType.Glitch;

    public Task HandleAsync(VitoCommandContext context, CancellationToken token)
    {
        VitoMemory.TriggerGlitch();
        context.ShowGlitch();

        context.Respond(new VitoResponse
        {
            Text = "Командир Неисправность подключён. Реальность искажена.",
            Sound = VitoSounds.GlitchBuzz,
            Vibration = true
        });

        return Task.CompletedTask;
    }
}

