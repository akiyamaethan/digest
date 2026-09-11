using System;
using UnityEngine;

/// Central event bus for decoupled cross-system communication.
public static class GameEvents
{
    // ============ HUNGER EVENTS ============
    public static event Action<float> onHungerGain;
    public static void OnHungerGain(float amount) => onHungerGain?.Invoke(amount);

    public static event Action<float> onHungerSet;
    public static void OnHungerSet(float amount) => onHungerSet?.Invoke(amount);

    // Fired when total hunger level is updated - for UI display
    public static event Action<float> onHungerUpdated;
    public static void OnHungerUpdated(float currentHunger) => onHungerUpdated?.Invoke(currentHunger);

    // Fired when hunger reaches 0 - triggers starvation game over
    public static event Action onHungerDepleted;
    public static void OnHungerDepleted() => onHungerDepleted?.Invoke();

    // ============ SCORE EVENTS ============
    public static event Action<int> onScoreGain;
    public static void OnScoreGain(int amount) => onScoreGain?.Invoke(amount);

    // Fired when total score is updated - for UI display
    public static event Action<int> onScoreUpdated;
    public static void OnScoreUpdated(int totalScore) => onScoreUpdated?.Invoke(totalScore);

    // Request to check and update high score (at game over)
    public static event Action onCheckHighScore;
    public static void OnCheckHighScore() => onCheckHighScore?.Invoke();

    // ============ HP EVENTS ============
    public static event Action<int, int> onHPChanged;
    public static void OnHPChanged(int newHP, int delta = 0) => onHPChanged?.Invoke(newHP, delta);

    // ============ ROUND/HOOK EVENTS ============
    public static event Action<int> onRoundChange;
    public static void OnRoundChange(int newRound) => onRoundChange?.Invoke(newRound);

    // Request to spawn the next hook
    public static event Action onSpawnNextHookRequested;
    public static void OnSpawnNextHookRequested() => onSpawnNextHookRequested?.Invoke();

    // ============ GAME STATE EVENTS ============
    public static GameOverCause lastGameOverCause { get; private set; } = GameOverCause.Caught;

    public static event Action<GameState> onGameStateChanged;
    public static void OnGameStateChanged(GameState newState) => onGameStateChanged?.Invoke(newState);

    public static event Action onGameOver;
    public static event Action<GameOverCause> onGameOverWithCause;

    public static void OnGameOver(GameOverCause cause = GameOverCause.Caught)
    {
        lastGameOverCause = cause;
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

    // ============ CLEANUP ============
    /// Clears all event subscriptions. Call this on scene unload to prevent memory leaks.
    public static void ClearAllEvents()
    {
        onHungerGain = null;
        onHungerSet = null;
        onHungerUpdated = null;
        onHungerDepleted = null;
        onScoreGain = null;
        onScoreUpdated = null;
        onCheckHighScore = null;
        onHPChanged = null;
        onRoundChange = null;
        onSpawnNextHookRequested = null;
        onGameStateChanged = null;
        onGameOver = null;
        onGameOverWithCause = null;
        lastGameOverCause = GameOverCause.Caught;
        onPauseRequested = null;
        onResumeRequested = null;
        onResetStateRequested = null;
        onPlaySound = null;
    }
}

public enum GameOverCause
{
    Caught,
    Starved
}
