using UnityEngine;

/// Manages power-up spawning based on round progression.
public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private GameObject heartFishPrefab;
    private int lastSpawnedRound = 0;

    void Awake()
    {
        GameEvents.onRoundChange += HandleRoundChange;
    }

    void OnDestroy()
    {
        GameEvents.onRoundChange -= HandleRoundChange;
    }

    private void HandleRoundChange(int newRound)
    {
        // Spawn heart fish every 10 rounds
        if (newRound % 10 == 0 && newRound != 0 && lastSpawnedRound != newRound)
        {
            SpawnNewHeartFish();
            lastSpawnedRound = newRound;
        }
    }

    private void SpawnNewHeartFish()
    {
        GameObject newFish = Instantiate(heartFishPrefab);
        AutoSwim currentFishScript = newFish.GetComponent<AutoSwim>();
        int coinToss = Random.Range(0, 2);
        float height = Random.Range(-2f, 2f);
        Debug.Log("Spawning heart fish at height: " + height.ToString());

        if (coinToss == 0)
        {
            currentFishScript.initalize("left", height);
        }
        else
        {
            currentFishScript.initalize("right", height);
        }
    }
}
