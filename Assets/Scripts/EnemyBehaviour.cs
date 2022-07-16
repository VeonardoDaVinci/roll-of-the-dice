using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public SpriteRenderer sprites;
    public Sprite sprite1;
    public Sprite sprite2;

    private void Start()
    {
        sprites = GameObject.FindGameObjectWithTag("EnemyAnim").GetComponent<SpriteRenderer>();
    }

    public void ChangeSprite()
    {
        if (sprites.sprite == sprite1)
        {
            sprites.sprite = sprite2;
        }
        else if (sprites.sprite == sprite2)
        {
            sprites.sprite = sprite1;

        }
    }
}
