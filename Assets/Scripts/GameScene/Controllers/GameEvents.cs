using System;

// Central event bus for decoupled cross-system communication.
public static class GameEvents
{
    // ============ HUNGER EVENTS ============
    public static event Action<float> onHungerGain;
    public static void OnHungerGain(float amount) => onHungerGain?.Invoke(amount);

    // Fired when total hunger level is updated - for UI display
    public static event Action<float> onHungerUpdated;
    public static void OnHungerUpdated(float currentHunger) => onHungerUpdated?.Invoke(currentHunger);


    // ============ SCORE EVENTS ============
    public static event Action<int> onScoreGain;
    public static void OnScoreGain(int amount) => onScoreGain?.Invoke(amount);

    // Fired when total score is updated - for UI display
    public static event Action<int> onScoreUpdated;
    public static void OnScoreUpdated(int totalScore) => onScoreUpdated?.Invoke(totalScore);


    // ============ HP EVENTS ============
    public static event Action<int, int> onHPChanged;
    public static void OnHPChanged(int newHP, int delta = 0) => onHPChanged?.Invoke(newHP, delta);


    // ============ ROUND/HOOK EVENTS ============
    public static event Action<int> onRoundChange;
    public static void OnRoundChange(int newRound) => onRoundChange?.Invoke(newRound);

    public static event Action onSpawnNextHookRequested;
    public static void OnSpawnNextHookRequested() => onSpawnNextHookRequested?.Invoke();


    // ============ GAME STATE EVENTS ============

    public static event Action<GameState> onGameStateChanged;
    public static void OnGameStateChanged(GameState newState) => onGameStateChanged?.Invoke(newState);

    public static event Action onGameOver;
    public static event Action<GameOverCause> onGameOverWithCause;

    public static void OnGameOver(GameOverCause cause = GameOverCause.Caught)
    {
        onGameOverWithCause?.Invoke(cause);
        onGameOver?.Invoke();
    }

    // Request events for pause control
    public static event Action onPauseRequested;
    public static void OnPauseRequested() => onPauseRequested?.Invoke();

    public static event Action onResumeRequested;
    public static void OnResumeRequested() => onResumeRequested?.Invoke();

    public static event Action onResetStateRequested;
    public static void OnResetStateRequested() => onResetStateRequested?.Invoke();


    // ============ SOUND EVENTS ============
    public static event Action<SoundName, float> onPlaySound;
    public static void OnPlaySound(SoundName sound, float volume = 1f) => onPlaySound?.Invoke(sound, volume);
}

public enum GameOverCause
{
    Caught,
    Starved
}
