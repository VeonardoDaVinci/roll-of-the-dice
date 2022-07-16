using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : SingletonPersistent<GameManager>
{
    public string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public TextMeshProUGUI letterObject;
    public char currentChar;
    private int letterIndex = 0;
    public char pressedChar;

    private TextMeshProUGUI wordObject;
    public string currentWord;
    private int wordIndex = 0;

    public int score = 0;

    private float startTime;
    private bool pressed;



    private int diceCount;

    private void Start()
    {
        letterObject = GameObject.FindGameObjectWithTag("Letter").GetComponent<TextMeshProUGUI>();
        wordObject = GameObject.FindGameObjectWithTag("Word").GetComponent<TextMeshProUGUI>();
        ChangeWord();
        InvokeRepeating("ChangeLetter", 2f, 2f);
    }

    private void ChangeWord()
    {
        System.Random rd = new System.Random();
        wordIndex = rd.Next(0, 999);
        wordObject.text = Wordlist.SharedInstance.wordList[wordIndex].Replace(" ", "");
        currentWord = Wordlist.SharedInstance.wordList[wordIndex].Replace(" ", "");
    }

    private bool CheckForLetterInTime()
    {

        if (Time.time - startTime <= 2f && !pressed)
        {
            if (pressedChar == currentChar)
            {

                score += 100;
                pressed = true;
                return true;
            }
        }
        else if (Time.time - startTime > 2f)
        {
            return false;
        }
        return false;
    }

    private void ChangeLetter()
    {
        //letter.text = alphabet[letterIndex].ToString();
        pressedChar = ' ';
        pressed = false;
        currentChar = currentWord[letterIndex];
        letterObject.text = currentChar.ToString();

        startTime = Time.time;
        letterIndex++;
        letterIndex %= currentWord.Length-1;


    }


    private void Update()
    {
        CheckForLetterInTime();
        Debug.Log(score);
    }
}
