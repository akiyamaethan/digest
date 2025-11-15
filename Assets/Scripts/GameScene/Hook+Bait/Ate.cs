using UnityEngine;


public class Ate : MonoBehaviour
{
    private float hungerGain = .6f;


    SpriteRenderer sprite;
    Texture2D originalTex;
    Texture2D dynamicTex;


    private int pixelsEaten = 0;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        originalTex = sprite.sprite.texture;
        

        dynamicTex = new Texture2D(originalTex.width, originalTex.height, originalTex.format, false);
        Graphics.CopyTexture(originalTex, dynamicTex);

        sprite.sprite = Sprite.Create(dynamicTex, new Rect(0,0, dynamicTex.width, dynamicTex.height), new Vector2(0.5f, 0.5f));
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
                        if (pixelsEaten % 15 == 0)
                        {
                            HungerManager.instance.alterHunger(hungerGain);
                            ScoreManager.instance.updateScore(1);
                        }
                        if (pixelsEaten % 1200 == 0)
                        {
                            transform.parent.GetComponent<HookSwing>()?.OnBaitEaten();
                        }
                            
                    }    
                }
            }
        }
        dynamicTex.Apply();
    }


}
