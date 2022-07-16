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
    public int letterIndex = 0;
    public char pressedChar;

    private TextMeshProUGUI wordObject;
    public string currentWord = "ceasar";
    private int wordIndex = 0;

    public int score = 0;

    private float startTime;
    private bool pressed;

    public int succesfulButtoPresses = -1;

    private GameObject[] dice;
    public int diceCount = 0;

    private void Start()
    {
        dice = GameObject.FindGameObjectsWithTag("Die");
        letterObject = GameObject.FindGameObjectWithTag("Letter").GetComponent<TextMeshProUGUI>();
        wordObject = GameObject.FindGameObjectWithTag("Word").GetComponent<TextMeshProUGUI>();
        ChangeWord();
        Debug.Log(succesfulButtoPresses);
        InvokeRepeating("ChangeLetter", 2f, 2f);
        
        for(int die=0; die<dice.Length; die++)
        {
            dice[die].SetActive(false);
        }

    }

    private void ChangeWord()
    {
        if (succesfulButtoPresses == currentWord.Length)
        {
            diceCount++;
            dice[diceCount - 1].SetActive(true);
        }
        //Debug.Log(succesfulButtoPresses);
        succesfulButtoPresses = 0;

        System.Random rd = new System.Random();
        wordIndex = rd.Next(0, 999);
        wordObject.text = Wordlist.SharedInstance.wordList[wordIndex].Replace(" ", "");
        currentWord = Wordlist.SharedInstance.wordList[wordIndex].Replace(" ", "");
    }

    private bool CheckForLetterInTime()
    {

        if (Time.time - startTime <= 2f && !pressed)
        {
            if (pressedChar == currentChar && currentChar != ' ')
            {
                succesfulButtoPresses++;
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

        pressedChar = ' ';
        pressed = false;

        if (letterIndex >= currentWord.Length)
        {
            letterIndex = 0;
            ChangeWord();
        }

        currentChar = currentWord[letterIndex];
        letterObject.text = currentChar.ToString();

        startTime = Time.time;
        letterIndex++;


    }


    private void Update()
    {
        CheckForLetterInTime();
        Debug.Log(succesfulButtoPresses);
        Debug.Log(currentWord.Length);
        //Debug.Log(dice.Length);
    }
}
