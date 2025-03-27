using VitoBox.Constants;

namespace VitoBox.Models;

public class VitoChatRequest
{
    public string Model { get; set; } = default!;
    public object[] Messages { get; set; } = default!;

    public static VitoChatRequest FromPrompt(string prompt, string model)
    {
        return new VitoChatRequest
        {
            Model = model,
            Messages =
            [
                new { role = "system", content = VitoConstants.SystemPrompt },
                new { role = "user", content = prompt }
            ]
        };
    }
}
