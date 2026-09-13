using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// Manages hook spawning and round progression.
// Listens to spawn request events and fires round change events.
public class HookManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject hookPrefab;

    private int roundNumber = 0;

    private const int EASY_ROUND_MAX = 15;
    private const int MEDIUM_ROUND_MAX = 40;
    private const int HARD_ROUND_MAX = 75;
    private const int EASY_ROUND_COIN_TOSS_MAX = 4;
    private const int MEDIUM_ROUND_COIN_TOSS_MAX = 3;
    private const int HARD_ROUND_COIN_TOSS_MAX = 2;
    private const float SPAWN_DELAY = 2f;

    private static readonly WaitForSeconds SpawnWait = new WaitForSeconds(SPAWN_DELAY);

    void Awake()
    {
        GameEvents.onSpawnNextHookRequested += HandleSpawnNextHookRequested;
    }

    void OnDestroy()
    {
        GameEvents.onSpawnNextHookRequested -= HandleSpawnNextHookRequested;
    }

    void Start()
    {
        SpawnNewHook();
        GameEvents.OnHungerSet(100f);  // Initialize hunger via event
    }

    private void HandleSpawnNextHookRequested()
    {
        SpawnNextHook();
    }

    public void SpawnNextHook()
    {
        int coinTossMax;
        if (roundNumber < EASY_ROUND_MAX)
            coinTossMax = EASY_ROUND_COIN_TOSS_MAX;
        else if (roundNumber < MEDIUM_ROUND_MAX)
            coinTossMax = MEDIUM_ROUND_COIN_TOSS_MAX;
        else
            coinTossMax = HARD_ROUND_COIN_TOSS_MAX;

        SpawnNewHook();

        int coinToss = Random.Range(1, coinTossMax);
        if (coinToss == 1)
            StartCoroutine(WaitThenSpawn());
    }

    public void SpawnNewHook()
    {
        roundNumber++;
        Debug.Log("Round Number: " + roundNumber);

        // Fire round change event for PowerUpManager and other listeners
        GameEvents.OnRoundChange(roundNumber);

        Instantiate(hookPrefab);
    }

    public IEnumerator WaitThenSpawn(float seconds = SPAWN_DELAY)
    {
        if (Mathf.Approximately(seconds, SPAWN_DELAY))
            yield return SpawnWait;
        else
            yield return new WaitForSeconds(seconds);

        SpawnNewHook();
    }
}
