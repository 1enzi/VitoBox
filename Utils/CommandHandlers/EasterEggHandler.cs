using VitoBox.Constants;
using VitoBox.Models.Enums;
using VitoBox.Models.Interfaces;
using VitoBox.Models;

namespace VitoBox.Utils.CommandHandlers;

public class EasterEggHandler : IVitoCommandHandler
{
    public bool CanHandle(VitoCommandContext context) =>
        context.Type == VitoCommandType.EasterEgg;

    public Task HandleAsync(VitoCommandContext context, CancellationToken token)
    {
        context.Respond(VitoEasterEggs.SummonResponse);
        return Task.CompletedTask;
    }
}
