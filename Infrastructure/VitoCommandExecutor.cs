using VitoBox.Models.Enums;
using VitoBox.Models;
using VitoBox.Utils;
using VitoBox.Models.Interfaces;
using VitoBox.Utils.CommandHandlers;

namespace VitoBox.Infrastructure;

public class VitoCommandExecutor
{
    private readonly List<IVitoCommandHandler> _handlers;
    private readonly ISerialPort _serial;
    private readonly IVitoApiService _api;

    public VitoCommandExecutor(ISerialPort serial, IVitoApiService api)
    {
        _handlers =
        [
            new ShutdownCommandHandler(),
            new EasterEggHandler(),
            new LostMemoryHandler(),
            new RestoreMemoryHandler(),
            new PromptCommandHandler(),
            new GlitchCommandHandler(),
            new GlitchEscapeAttemptHandler(),
        ];

        _serial = serial;
        _api = api;
    }

    public async Task ExecuteAsync(VitoCommandType type, string? prompt, CancellationToken token)
    {
        var context = new VitoCommandContext
        {
            Type = type,
            Prompt = prompt,
            Serial = _serial,
            Api = _api
        };

        foreach (var handler in _handlers)
        {
            if (handler.CanHandle(context))
            {
                await handler.HandleAsync(context, token);
                return;
            }
        }

        Logger.LogWarning("Нет подходящего обработчика команды.");
    }
}