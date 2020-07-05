using UnityEngine;
using System.Collections;
[RequireComponent(typeof(SpriteRenderer))]

public class AutoStretchSprite : MonoBehaviour
{

    /// <summary> Do you want the sprite to maintain the aspect ratio? </summary>
    public bool KeepAspectRatio = true;
    /// <summary> Do you want it to continually check the screen size and update? </summary>
    public bool ExecuteOnUpdate = true;

    void Start()
    {
        Resize();
    }

    void Resize()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        transform.localScale = new Vector3(1, 1, 1);

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;


        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        Vector3 xWidth = transform.localScale;
        xWidth.x = worldScreenWidth / width;
        transform.localScale = xWidth;
        //transform.localScale.x = worldScreenWidth / width;
        Vector3 yHeight = transform.localScale;
        yHeight.y = worldScreenHeight / height;
        transform.localScale = yHeight;
        //transform.localScale.y = worldScreenHeight / height;
    }

}