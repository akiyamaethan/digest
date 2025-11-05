using UnityEngine;

public class HPManager : MonoBehaviour
{
    public static HPManager instance;
    private int _HP = 0;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void updateHP(int HP)
    {
        _HP = HP;
        HPBehavior.instance.updateHP(_HP);
    }

    public void blink()
    {
        HPBehavior.instance.blink();
    }
}
