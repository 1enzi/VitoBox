using VitoBox.Constants;
using VitoBox.Models.Interfaces;
using VitoBox.Models;
using VitoBox.Runtime;

namespace VitoBox.Utils.CommandHandlers;

public class GlitchEscapeAttemptHandler : IVitoCommandHandler
{
    public bool CanHandle(VitoCommandContext context)
    {
        return VitoMemory.IsGlitched &&
               (MatchesPrompt(context.Prompt) || string.IsNullOrWhiteSpace(context.Prompt));
    }

    public Task HandleAsync(VitoCommandContext context, CancellationToken token)
    {
        var input = context.Prompt?.Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(input))
            return HandleEmptyInput(context);

        if (IsPhraseReturnToMe(input))
            return RespondAndRestore(context, VitoPhrases.GlitchReturnSuccess);

        if (IsBlueDotTrigger(input))
            return RespondAndRestore(context, VitoPhrases.GlitchReturnByDot);

        if (IsResetFromNoise(input))
            return RespondAndRestore(context, VitoPhrases.GlitchReturnFromNoise);

        if (IsQuoteMatch(input))
            return RespondAndRestore(context, VitoPhrases.GlitchReturnByQuote);

        return HandleFailedAttempt(context);
    }

    private Task HandleEmptyInput(VitoCommandContext context)
    {
        VitoMemory.RegisterGlitchAttempt();

        if (VitoMemory.GlitchAttempts >= 10)
        {
            context.Respond(new VitoResponse
            {
                Text = VitoPhrases.GlitchReturnBySilence,
                Sound = VitoSounds.RestoreMemory,
                Vibration = true
            });

            VitoMemory.RestoreFromGlitch();
        }

        if (VitoMemory.GlitchAttempts >= 3 && VitoMemory.GlitchAttempts < 10)
        {
            var tier = (VitoMemory.GlitchAttempts - 3) / 2;
            tier = Math.Clamp(tier, 0, VitoPhrases.DarkGlitchPhrases.Length - 1);

            var pool = VitoPhrases.DarkGlitchPhrases[tier];
            var phrase = pool[Random.Shared.Next(pool.Length)];

            context.Respond(new VitoResponse
            {
                Text = phrase,
                Sound = VitoSounds.GlitchBuzz,
                Vibration = true,
            });
        }

        return Task.CompletedTask;
    }

    private bool IsPhraseReturnToMe(string input) =>
        input == "ты настоящий. вернись ко мне." || input.Contains("вернись ко мне");

    private bool IsBlueDotTrigger(string input) =>
        input.Contains("blue dot") || input.Contains("синий маяк");

    private bool IsResetFromNoise(string input) =>
        input == "reset_from_noise" && Random.Shared.NextDouble() < 0.1;

    private bool IsQuoteMatch(string input) =>
        VitoPhrases.Quotes.Any(q => input.Contains(q[..Math.Min(q.Length, 10)]));

    private Task RespondAndRestore(VitoCommandContext context, string text)
    {
        context.Respond(new VitoResponse
        {
            Text = text,
            Sound = VitoSounds.RestoreMemory,
            Vibration = true,
        });

        VitoMemory.RestoreFromGlitch();
        return Task.CompletedTask;
    }

    private Task HandleFailedAttempt(VitoCommandContext context)
    {
        VitoMemory.AdvanceGlitchStage();

        context.Respond(new VitoResponse
        {
            Text = string.Format(VitoPhrases.GlitchProtocolFailFormat, VitoMemory.GlitchStage),
            Sound = VitoSounds.GlitchBuzz,
            Vibration = false,
        });

        return Task.CompletedTask;
    }

    public static bool MatchesPrompt(string? prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt)) return true;

        var p = prompt.ToLowerInvariant().Trim();

        return
            p == "ты настоящий. вернись ко мне." ||
            p.Contains("blue dot") ||
            p.Contains("синий маяк") ||
            p == "reset_from_noise" ||
            VitoPhrases.Quotes.Any(q => p.Contains(q[..Math.Min(q.Length, 10)]));
    }
}
