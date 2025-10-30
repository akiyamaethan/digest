using System.Xml.Schema;
using UnityEngine;

public class Ate : MonoBehaviour
{
    [SerializeField] private bool debugMode = false;
    [SerializeField] private float startHunger = 50f;
    [SerializeField] private float hungerGain = 5f;
    [SerializeField] private float hungerDrain = 0.1f;
    [SerializeField] private int hungerDrainInterval = 10;
    [SerializeField] private HungerBar hungerBar;

    SpriteRenderer sprite;
    Texture2D originalTex;
    Texture2D dynamicTex;
    private int pixelsEaten = 0;
    private static int TOTALPX = 40000;
    void Start()
    {
        
        sprite = GetComponent<SpriteRenderer>();
        originalTex = sprite.sprite.texture;

        dynamicTex = new Texture2D(originalTex.width, originalTex.height, originalTex.format, false);
        Graphics.CopyTexture(originalTex, dynamicTex);

        sprite.sprite = Sprite.Create(dynamicTex, new Rect(0,0, dynamicTex.width, dynamicTex.height), new Vector2(0.5f, 0.5f));
        hungerBar.setHunger(startHunger);
    }

    // Update is called once per frame
    void Update()
    {
        if (hungerDrainInterval == 10)
        {
            hungerDrainInterval = 0;
            hungerBar.decrementHunger(hungerDrain);
        }
        else
        {
            hungerDrainInterval += 1;
        }
        
    }

    public void Cut(Collider2D collider)
    {
        Bounds personalBounds = sprite.bounds;
        Bounds cutterBounds = collider.bounds;

        int xMin = (int)Mathf.Max(0, (cutterBounds.min.x - personalBounds.min.x) / personalBounds.size.x * dynamicTex.width);
        int yMin = (int)Mathf.Max(0, (cutterBounds.min.y - personalBounds.min.y) / personalBounds.size.y * dynamicTex.height);
        int xMax = (int)Mathf.Min(dynamicTex.width, (cutterBounds.max.x - personalBounds.min.x) / personalBounds.size.x * dynamicTex.width);
        int yMax = (int)Mathf.Min(dynamicTex.height, (cutterBounds.max.y - personalBounds.min.y) / personalBounds.size.y * dynamicTex.height);


        for (int y = yMin; y<yMax;y++)
        {
            for (int x = xMin; x<xMax; x++)
            {
                Vector2 local = new Vector2(
                    personalBounds.min.x + (x / (float)dynamicTex.width) * personalBounds.size.x,
                    personalBounds.min.y + (y / (float)dynamicTex.height) * personalBounds.size.y
                    );

                if (collider.OverlapPoint(local))
                {
                    if (dynamicTex.GetPixel(x, y).a > 0)
                    {
                        dynamicTex.SetPixel(x, y, new Color(0, 0, 0, 0));
                        pixelsEaten++;
                        if (pixelsEaten % 500 == 0 && debugMode)
                        {
                            Debug.Log(((float)pixelsEaten/TOTALPX) * 100+"%");
                            hungerBar.incrementHunger(hungerGain);
                        }
                    }    
                }
            }
        }
        dynamicTex.Apply();
    }
}
