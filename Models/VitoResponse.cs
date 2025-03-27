namespace VitoBox.Models;

public class VitoResponse
{
    public string Text { get; set; } = string.Empty;
    public string? Sound { get; set; }
    public bool Vibration { get; set; } = false;
}
