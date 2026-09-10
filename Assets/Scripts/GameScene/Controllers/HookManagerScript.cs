using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Manages hook spawning and round progression.
/// Listens to spawn request events and fires round change events.
/// Decoupled from UI and individual hook parameters.
/// </summary>
public class HookManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject hookPrefab;

    public List<GameObject> activeHooks { get; private set; } = new List<GameObject>();
    private int roundNumber = 0;

    private const int EASY_ROUND_MAX = 15;
    private const int MEDIUM_ROUND_MAX = 40;
    private const int EASY_ROUND_COIN_TOSS_MAX = 4;
    private const int MEDIUM_ROUND_COIN_TOSS_MAX = 3;
    private const float SPAWN_DELAY = 2f;

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
        GameEvents.OnHPChange(3);  // Initialize HP display via event
    }

    private void HandleSpawnNextHookRequested()
    {
        SpawnNextHook();
    }

    public void SpawnNextHook()
    {
        int coinToss = Random.Range(1, EASY_ROUND_COIN_TOSS_MAX);
        if (roundNumber < EASY_ROUND_MAX)
        {
            SpawnNewHook();
            if (coinToss == 1)
                StartCoroutine(WaitThenSpawn(SPAWN_DELAY));
            return;
        }
        if (EASY_ROUND_MAX <= roundNumber && roundNumber < MEDIUM_ROUND_MAX)
        {
            SpawnNewHook();
            if (coinToss < MEDIUM_ROUND_COIN_TOSS_MAX)
                StartCoroutine(WaitThenSpawn(SPAWN_DELAY));
            return;
        }
    }

    public void SpawnNewHook()
    {
        roundNumber++;
        Debug.Log("Round Number: " + roundNumber);

        // Fire round change event for PowerUpManager and other listeners
        GameEvents.OnRoundChange(roundNumber);

        GameObject newHook = Instantiate(hookPrefab);
        activeHooks.Add(newHook);
    }

    public void SetEaten(GameObject hook)
    {
        if (hook == null)
            return;
        HookSwing currentHookScript = hook.GetComponent<HookSwing>();
        if (currentHookScript != null)
            currentHookScript.baitEaten = true;
    }

    public IEnumerator WaitThenSpawn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SpawnNewHook();
    }
}
