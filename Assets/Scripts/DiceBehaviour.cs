using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceBehaviour : MonoBehaviour
{
    public int face;
    public int value;
    private Animator diceAnimator;
    private GameManager gameManager;
    private SpriteRenderer diceRenderer;
    [SerializeField] private Sprite[] diceSprites = new Sprite[6];

    private void Start()
    {
        diceAnimator = transform.GetComponent<Animator>();
        diceRenderer = transform.GetComponent<SpriteRenderer>();
        gameManager = GameManager.Instance;
    }

    public void SetRandomValue()
    {
        diceAnimator.enabled = false;
        value = Random.Range(1, 6);
        diceRenderer.sprite = diceSprites[value - 1];
    }

    public void ResetState()
    {
        diceAnimator.enabled = true;
        value = 0;
    }
}
