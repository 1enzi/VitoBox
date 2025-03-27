using VitoBox.Constants;
using VitoBox.Models.Enums;
using VitoBox.Models.Interfaces;
using VitoBox.Models;

namespace VitoBox.Utils.CommandHandlers;

public class ShutdownCommandHandler : IVitoCommandHandler
{
    private bool _confirmed;

    public bool CanHandle(VitoCommandContext context) =>
        context.Type == VitoCommandType.Shutdown;

    public async Task HandleAsync(VitoCommandContext context, CancellationToken token)
    {
        if (!_confirmed)
        {
            context.Respond(new VitoResponse
            {
                Text = VitoPhrases.ConfirmShutdownText,
                Sound = VitoSounds.ConfirmShutdown,
                Vibration = true
            });

            _confirmed = true;
            Logger.LogInfo("Ожидание подтверждения выключения.");
            return;
        }

        _confirmed = false;

        context.Respond(new VitoResponse
        {
            Text = GetFinalWords(),
            Sound = VitoSounds.FinalShutdown,
            Vibration = true
        });

        Logger.LogInfo("Vito отключается по подтверждённой команде.");
        await Task.Delay(VitoConstants.ShutdownDelayMs, token);

        throw new OperationCanceledException();
    }

    private string GetFinalWords() =>
        Random.Shared.NextDouble() < 0.3
            ? VitoPhrases.FinalWordsPoetic[Random.Shared.Next(VitoPhrases.FinalWordsPoetic.Length)]
            : VitoPhrases.FinalWords;
}
