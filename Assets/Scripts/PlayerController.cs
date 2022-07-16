using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;
    private void Start()
    {
        gameManager = GameManager.Instance;
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
