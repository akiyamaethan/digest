using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class HookManagerScript : MonoBehaviour
{
    public Canvas canvas;
    public static HookManagerScript instance;
    public GameObject hookPrefab;
    public FishFollowMouse player;
    public TMP_Text gameOver;
    public TMP_Text youStarved;
    public TMP_Text youGotCaught;
    public GameObject reset;

    public List<GameObject> activeHooks = new List<GameObject>();
    private int roundNumber = 0;

    private void Awake()
    {
        if (instance == null)   
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        spawnNewHook();
        HungerManager.instance.setHunger(100f);
        HPManager.instance.updateHP(3);
    }


    public void spawnNextHook()
    {
 
        int coinToss = Random.Range(1, 4); //1 to 3
        Debug.Log("coinflip: "+coinToss);
        if (roundNumber < 15)
        {
            spawnNewHook();
            if (coinToss == 1)
                StartCoroutine(waitThenSpawn(2));
            return;
        }
        if (15 <= roundNumber && roundNumber < 40)
        {
            spawnNewHook();
            if (coinToss == 1 || coinToss == 2)
                StartCoroutine(waitThenSpawn(2f));
            return;
        }
    }

    public void spawnNewHook()
    {
        roundNumber++;
        Debug.Log("Round Number: " + roundNumber);

        GameObject newHook = Instantiate(hookPrefab);
        HookSwing currentHookScript = newHook.GetComponent<HookSwing>();
        currentHookScript.initialize(player, gameOver, youStarved, youGotCaught, reset, canvas);
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
