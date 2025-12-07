using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class HookManagerScript : SingletonNoPersist<HookManagerScript>
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject hookPrefab;
    [SerializeField] private PointPlayerMovement player;
    [SerializeField] private TMP_Text gameOver;
    [SerializeField] private TMP_Text youStarved;
    [SerializeField] private TMP_Text youGotCaught;
    [SerializeField] private GameObject reset;
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject gameOverHighScore;

    public List<GameObject> activeHooks { get; private set; } = new List<GameObject>();
    private int roundNumber = 0;

    protected override void Awake()
    {
        base.Awake();  // Handle singleton logic
    }
    void Start()
    {
        spawnNewHook();
        HungerManager.instance.setHunger(100f);
        HPManager.instance.updateHP(3);
    }


    private const int EASY_ROUND_MAX = 15;
    private const int MEDIUM_ROUND_MAX = 40;
    private const int EASY_ROUND_COIN_TOSS_MAX = 4;
    private const int MEDIUM_ROUND_COIN_TOSS_MAX = 3;
    private const float SPAWN_DELAY = 2f;

    public void spawnNextHook()
    {
        int coinToss = Random.Range(1, EASY_ROUND_COIN_TOSS_MAX);
        if (roundNumber < EASY_ROUND_MAX)
        {
            spawnNewHook();
            if (coinToss == 1)
                StartCoroutine(waitThenSpawn(SPAWN_DELAY));
            return;
        }
        if (EASY_ROUND_MAX <= roundNumber && roundNumber < MEDIUM_ROUND_MAX)
        {
            spawnNewHook();
            if (coinToss < MEDIUM_ROUND_COIN_TOSS_MAX)
                StartCoroutine(waitThenSpawn(SPAWN_DELAY));
            return;
        }
    }

    public void spawnNewHook()
    {
        roundNumber++;
        Debug.Log("Round Number: " + roundNumber);

        GameObject newHook = Instantiate(hookPrefab);
        HookSwing currentHookScript = newHook.GetComponent<HookSwing>();
        HookInitializationData data = new HookInitializationData
        {
            player = player,
            gameOver = gameOver,
            youStarved = youStarved,
            youGotCaught = youGotCaught,
            restartButton = reset,
            mainCanvas = canvas,
            title = title,
            gameOverHighScore = gameOverHighScore
        };
        currentHookScript.initialize(data);
        activeHooks.Add(newHook);
    }

    public void setEaten(GameObject hook)
    {
        if (hook == null)
            return;
        HookSwing currentHookScript = hook.GetComponent<HookSwing>();
        if (currentHookScript != null)
            currentHookScript.baitEaten = true;
    }

    public IEnumerator waitThenSpawn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        spawnNewHook();
    }

}
