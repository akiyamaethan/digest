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
}
