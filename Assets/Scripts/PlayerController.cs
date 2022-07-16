using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;
    private Animation anim;
    private void Start()
    {
        anim = GetComponent<Animation>();
        gameManager = GameManager.Instance;
        //anim.Play("cezar-anim");
        //anim["cezar-anim"].speed = gameManager.bpm/(60*4);
        anim["cezar-anim"].speed = 0.1f;
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
