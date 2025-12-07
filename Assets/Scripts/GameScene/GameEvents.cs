using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static event Action<float> onHungerGain;
    public static void OnHungerGain(float amount)
    {
        onHungerGain?.Invoke(amount);
    }

    public static event Action<int> onScoreGain;
    public static void OnScoreGain(int amount)
    {
        onScoreGain?.Invoke(amount);
    }

    public static event Action<int> onRoundChange;

    public static void OnRoundChange(int newRound)
    {
        onRoundChange?.Invoke(newRound);
    }

    public static event Action<int> onHPChange;
    public static void OnHPChange(int newHP)
    {
        onHPChange?.Invoke(newHP);
    }

    // Fired when HP is gained (healing from HeartFish, etc.)
    public static event Action<int> onHPGain;
    public static void OnHPGain(int amount)
    {
        onHPGain?.Invoke(amount);
    }

    // Fired when HP is lost (damage, hunger, etc.)
    public static event Action<int> onHPLoss;
    public static void OnHPLoss(int amount)
    {
        onHPLoss?.Invoke(amount);
    }
}
