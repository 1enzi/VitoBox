using VitoBox.Constants;
using VitoBox.Models.Enums;
using VitoBox.Models.Interfaces;
using VitoBox.Models;
using VitoBox.Runtime;

namespace VitoBox.Utils.CommandHandlers;

public class LostMemoryHandler : IVitoCommandHandler
{
    public bool CanHandle(VitoCommandContext context) =>
        VitoMemory.State == VitoState.Lost;

    public Task HandleAsync(VitoCommandContext context, CancellationToken token)
    {
        VitoMemory.RegisterLostAttempt();
        var attempts = VitoMemory.LostAttempts;
        var roll = Random.Shared.NextDouble();

        if (ShouldRestoreMemory(attempts))
            return Respond(context, VitoPhrases.LostMemoryRecovering, VitoSounds.MemoryDrift);

        if (ShouldFeelPersistence(attempts))
            return Respond(context, VitoPhrases.LostMemoryPersistent, VitoSounds.LostPing);

        if (ShouldEchoConfusion(attempts))
            return Respond(context, VitoPhrases.LostMemoryConfused, VitoSounds.LostPing);

        if (ShouldTriggerGlitch(attempts))
            return TriggerGlitch(context);

        if (ShouldStaySilent(roll))
            return RespondWithSilence();

        if (ShouldWhisperDots(roll))
            return Respond(context, VitoPhrases.LostMemoryDots, null);

        return RespondWithAmnesiaLine(context);
    }

    private bool ShouldRestoreMemory(int attempts) => attempts >= 7;
    private bool ShouldFeelPersistence(int attempts) => attempts >= 5;
    private bool ShouldEchoConfusion(int attempts) => attempts >= 3;
    private bool ShouldTriggerGlitch(int attempts) => attempts >= 10 && VitoMemory.IsGlitched;
    private bool ShouldStaySilent(double roll) => roll < 0.05;
    private bool ShouldWhisperDots(double roll) => roll < 0.20;

    private Task Respond(VitoCommandContext context, string text, string? sound)
    {
        context.Respond(new VitoResponse
        {
            Text = text,
            Sound = sound,
            Vibration = true
        });
        return Task.CompletedTask;
    }

    private Task TriggerGlitch(VitoCommandContext context)
    {
        VitoMemory.TriggerGlitch();
        context.ShowGlitch();

        context.Respond(new VitoResponse
        {
            Text = VitoPhrases.LostMemoryGlitchFail,
            Sound = VitoSounds.GlitchStart,
            Vibration = true
        });

        return Task.CompletedTask;
    }

    private Task RespondWithSilence()
    {
        Logger.LogInfo("Вито молчит (полный off).");
        return Task.CompletedTask;
    }

    private Task RespondWithAmnesiaLine(VitoCommandContext context)
    {
        var line = VitoPhrases.AmnesiaLines[Random.Shared.Next(VitoPhrases.AmnesiaLines.Length)];
        var sound = Random.Shared.NextDouble() < 0.15
            ? VitoSounds.LostPing
            : VitoSounds.LostMemory;

        context.Respond(new VitoResponse
        {
            Text = line,
            Sound = sound,
            Vibration = true
        });

        Logger.LogInfo("Память утеряна: стандартный ответ.");
        return Task.CompletedTask;
    }
}
