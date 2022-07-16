using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;
    public SpriteRenderer sprites;
    public Sprite sprite1;
    public Sprite sprite2;

    private void Start()
    {
        sprites = GameObject.FindGameObjectWithTag("PlayerAnim").GetComponent<SpriteRenderer>();
        gameManager = GameManager.Instance;
        //anim.Play("cezar-anim");
        //anim["cezar-anim"].speed = gameManager.bpm/(60*4);
        //sprites.speed = gameManager.bpm/(60*4);
    }

    public void ChangeSprite()
    {
        if (sprites.sprite == sprite1)
        {
            sprites.sprite = sprite2;
            
            return;
        }
        
        sprites.sprite = sprite1;
    }

    private void OnEnable()
    {
        Keyboard.current.onTextInput += OnTextInput;
    }

    private void OnDisable()
    {
        Keyboard.current.onTextInput -= OnTextInput;
    }

    private void OnTextInput(char ch)
    {
        //gameManager.letterObject.text = ch.ToString();
        gameManager.pressedChar = ch;
    }

    private void Update()
    {
      
    }
}
