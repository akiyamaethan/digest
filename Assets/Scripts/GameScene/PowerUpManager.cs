using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    HookManagerScript hookManager;
    [SerializeField] private GameObject heartFishPrefab;
    private int round;
    private int lastSpawnedRound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hookManager = HookManagerScript.instance;
    }

    // Update is called once per frame
    void Update()
    {
        round = hookManager.getRoundNumber();
        if (round % 3 == 0 && round != 0)
        {
            spawnNewHeartFish();
        }
    }

    public void spawnNewHeartFish()
    {
        if (lastSpawnedRound == round)
        {
            return;
        }

        GameObject newFish = Instantiate(heartFishPrefab);
        AutoSwim currentFishScript = newFish.GetComponent<AutoSwim>();
        int coinToss = Random.Range(0, 2);
        int height = Random.Range(-4, 4);
        if (coinToss == 0)
        {
            currentFishScript.initalize("left", height);
        }
        else
        {
            currentFishScript.initalize("right", height);
        }

        lastSpawnedRound = round;
    }
}
