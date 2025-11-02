using TMPro;
using UnityEngine;

public class HookManagerScript : MonoBehaviour
{

    public static HookManagerScript instance;


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

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
