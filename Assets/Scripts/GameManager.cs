using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using HealthBar;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : SingletonPersistent<GameManager>
{
    public string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public TextMeshProUGUI letterObject;
    public char currentChar;
    public int letterIndex = 0;
    public char pressedChar;

    public float bpm = 100;

    public RectTransform arrow;

    private AudioSource audioData;
    public AudioClip drum1;
    public AudioClip drum2;

    private TextMeshProUGUI wordObject;
    public string currentWord;
    private int wordIndex = 0;

    public int score = 0;

    private float startTime;
    private bool pressed = true;

    public int succesfulButtonPresses = 0;

    private GameObject[] dice;
    public int diceCount = 0;
    private IHealthBar _healthBar;

    private void Start()
    {
        LoadHealthBar();
        
        audioData = GetComponent<AudioSource>();
        dice = GameObject.FindGameObjectsWithTag("Die");
        letterObject = GameObject.FindGameObjectWithTag("Letter").GetComponent<TextMeshProUGUI>();
        wordObject = GameObject.FindGameObjectWithTag("Word").GetComponent<TextMeshProUGUI>();
        ChangeWord();
        InvokeRepeating("PlayRythm", 60f/bpm, 60f/bpm);
        InvokeRepeating("ChangeLetter", 30f/bpm, 30f/bpm);
        
        for(int die=0; die<dice.Length; die++)
        {
            dice[die].SetActive(false);
        }

    }

    private void ChangeWord()
    {
        if (succesfulButtonPresses == currentWord.Length-1)
        {
            Debug.Log(succesfulButtonPresses);
            Debug.Log(currentWord);
            diceCount++;
            dice[diceCount - 1].SetActive(true);
        }
        //Debug.Log(succesfulButtoPresses);
        succesfulButtonPresses = 0;
        arrow.position = new Vector2(537f, arrow.position.y);
        Debug.Log(arrow.position);
        System.Random rd = new System.Random();
        wordIndex = rd.Next(0, 999);
        currentWord = Wordlist.SharedInstance.wordList[wordIndex].Replace(" ", "");
        wordObject.text = currentWord;
    }

    private void PlayRythm()
    {
        audioData.PlayOneShot(drum2);
    }

    private bool CheckForLetterInTime()
    {

        if (Time.time - startTime <= 2f && !pressed)
        {
            if (pressedChar == currentChar && currentChar != ' ')
            {
                succesfulButtonPresses++;
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
        audioData.PlayOneShot(drum1);
        arrow.position = new Vector2(arrow.position.x+23f, arrow.position.y);
        if (letterIndex >= currentWord.Length)
        {
            letterIndex = 0;
            ChangeWord();
        }

        currentChar = currentWord[letterIndex];
        letterObject.text = currentChar.ToString();

        startTime = Time.time;
        letterIndex++;

        
            
        _healthBar.DecreaseHealth(); // tylko dla testu tutaj odpalam, bo nie wiem gdzie
    }
    
    private void LoadHealthBar()
    {
        _healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar.IHealthBar>();
    }
    
    private void Update()
    {
        CheckForLetterInTime();
        Debug.Log(succesfulButtonPresses);
        Debug.Log(currentWord.Length);
        //Debug.Log(dice.Length);
    }
}
