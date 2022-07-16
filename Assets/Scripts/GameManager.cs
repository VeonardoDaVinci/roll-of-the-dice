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
    private TextMeshProUGUI wordObject;
    public string currentWord;
    private int wordIndex = 0;
    public int score = 0;
    public char pressedChar;
    private float startTime;

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
        
        //if (Time.time - start_time <= 2f)
        //{
        //    if (pressedChar == currentChar)
        //    {

        //        score += 100;
        //        return true;
        //    }
        //}
        //else if (Time.time -start_time > 2f)
        //{
        //    return false;
        //}
        //return false;
        if (pressedChar == currentChar)
        {
            score += 100;
            return true;
        }
        return false;
    }

    private void ChangeLetter()
    {
        //letter.text = alphabet[letterIndex].ToString();
        pressedChar = ' ';
        currentChar = currentWord[letterIndex];
        letterObject.text = currentChar.ToString();

        startTime = Time.time;
        CheckForLetterInTime();
        letterIndex++;
        letterIndex %= currentWord.Length;


    }


    private void Update()
    {
        Debug.Log(startTime);
    }
}
