using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HookManagerScript : MonoBehaviour
{

    public static HookManagerScript instance;
    public GameObject currentHook;

    public GameObject hookPrefab;
  
    public FishFollowMouse player;
    public TMP_Text gameOver;
    public TMP_Text youStarved;
    public TMP_Text youGotCaught;
    public GameObject reset;

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
        prefabHookScript.initialize(player, gameOver, youStarved, youGotCaught, reset);
        HungerManager.instance.setHunger(100f);
        HPManager.instance.updateHP(3);
        currentHook = initialHook;
    }
    public void spawnNewHook()
    {
        GameObject newHook = Instantiate(hookPrefab);
        HookSwing currentHookScript = newHook.GetComponent<HookSwing>();
        currentHookScript.initialize(player, gameOver, youStarved, youGotCaught, reset);
        currentHook = newHook;
    }

    public void setEaten()
    {
        HookSwing currentHookScript = currentHook.GetComponent<HookSwing>();
        currentHookScript.baitEaten = true;
    }

}
