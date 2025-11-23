using UnityEngine;

public class HPManager : SingletonNoPersist<HPManager>
{
    private int _HP = 0;

    protected override void Awake()
    {
        base.Awake();  // Handle singleton logic
    }

    public void updateHP(int HP)
    {
        _HP = HP;
        if (HPBehavior.instance != null)
        {
            HPBehavior.instance.updateHP(_HP);
        }
        else
        {
            Debug.LogWarning("[HPManager] HPBehavior instance not found!");
        }
    }

    public void blink()
    {
        if (HPBehavior.instance != null)
        {
            HPBehavior.instance.blink();
        }
        else
        {
            Debug.LogWarning("[HPManager] HPBehavior instance not found!");
        }
    }
}
