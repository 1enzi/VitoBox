namespace VitoBox.Models.Interfaces;

public interface IVitoApiService
{
    Task<VitoResponse> GetVitoResponseAsync(string prompt, CancellationToken token);
}
