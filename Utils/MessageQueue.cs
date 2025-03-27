using System.Collections.Concurrent;

namespace VitoBox.Utils;

public class MessageQueue
{
    private readonly ConcurrentQueue<string> _queue = new();

    public void Enqueue(string message) => _queue.Enqueue(message);

    public bool TryDequeue(out string? message) => _queue.TryDequeue(out message);
}
