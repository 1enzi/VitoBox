namespace VitoBox.Models.Interfaces;

public interface IVitoCommandHandler
{
    bool CanHandle(VitoCommandContext context);
    Task HandleAsync(VitoCommandContext context, CancellationToken token);
}
