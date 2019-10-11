using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingBG : MonoBehaviour
{
    public Image bgImage;
    public SpriteRenderer bgSprite;
    public float movingSpeed;

    void Update()
    {
        if (bgImage != null)
        {
           // bgImage.size = new Vector2(bgSprite.size.x, bgSprite.size.y + movingSpeed);
        }
        else if (bgSprite != null)
        {
            bgSprite.size = new Vector2(bgSprite.size.x, bgSprite.size.y + movingSpeed);
        }
    }
}
