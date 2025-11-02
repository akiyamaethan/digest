using TMPro;
using UnityEngine;

public class HookManagerScript : MonoBehaviour
{

    public static HookManagerScript instance;
    public GameObject currentHook;


    public GameObject hookPrefab;
   
    //Hook swinger args
    public FishFollowMouse player;
    public TMP_Text gameOver;

    private void Awake()
    {
        if (instance == null)   
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        GameObject initialHook = Instantiate(hookPrefab);
        HookSwing prefabHookScript = initialHook.GetComponent<HookSwing>();
        prefabHookScript.initialize(player, gameOver);
        HungerManager.instance.setHunger(50f);
        currentHook = initialHook;

    }

    // Update is called once per frame
    public void spawnNewHook()
    {
        GameObject newHook = Instantiate(hookPrefab);
        HookSwing currentHookScript = newHook.GetComponent<HookSwing>();
        currentHookScript.initialize(player, gameOver);
        currentHook = newHook;
    }

    public void setEaten()
    {
        HookSwing currentHookScript = currentHook.GetComponent<HookSwing>();
        currentHookScript.baitEaten = true;
    }

}
