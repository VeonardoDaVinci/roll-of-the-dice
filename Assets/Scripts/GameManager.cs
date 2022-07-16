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

    public float bpm;

    public PlayerController player;
    public EnemyBehaviour enemy;

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

    private string gameState = "rythm";

    public int succesfulButtonPresses = 0;

    private GameObject[] dice;
    private GameObject[] enemyDice;

    public int diceCount = 0;
    private IHealthBar _healthBar;

    private void Start()
    {
        LoadHealthBar();

        bpm = 50;
        
        LoadComponentsFromScreen();
        
        SetWord();
        
        StartRythm();

        SetDiceAsNotActiveByDefault();

    }

    private void SetDiceAsNotActiveByDefault()
    {
        for (int die = 0; die < dice.Length; die++)
        {
            dice[die].SetActive(false);
        }
    }

    private void StartRythm()
    {
        InvokeRepeating(nameof(ChangeLetter), 60f / bpm, 60f / bpm);
        InvokeRepeating(nameof(PlayRythm), 30f / bpm, 30f / bpm);
    }

    private void LoadComponentsFromScreen()
    {
        audioData = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyBehaviour>();

        dice = GameObject.FindGameObjectsWithTag("Die");
        letterObject = GameObject.FindGameObjectWithTag("Letter").GetComponent<TextMeshProUGUI>();
        wordObject = GameObject.FindGameObjectWithTag("Word").GetComponent<TextMeshProUGUI>();
    }

    private void SetWord()
    {
        succesfulButtonPresses = 0;
        arrow.position = new Vector2(537f, arrow.position.y);
        Debug.Log(arrow.position);
        System.Random rd = new System.Random();
        wordIndex = rd.Next(0, 999);
        currentWord = Wordlist.SharedInstance.wordList[wordIndex].Replace(" ", "");
        wordObject.text = currentWord;
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
        else
        {
            ChangeState();
        }
        //Debug.Log(succesfulButtoPresses);
        if (gameState == "rythm")
        {
            SetWord();
        }
        else if (gameState == "hazard")
        {
            CancelInvoke();
            if (dice.Length > 0)
            {
                for (int a = 0; a <= dice.Length-1; a++)
                {
                    dice[a].GetComponent<DiceBehaviour>().SetRandomValue();
                }
            }
        }
    }

    private void ChangeState()
    {
        gameState = "hazard";
    }

    private void PlayRythm()
    {
        audioData.PlayOneShot(drum2);
    }

    private bool CheckForLetterInTime()
    {

        if (Time.time - startTime <= (60f/bpm) && !pressed)
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
        AnimateCharactersMovement();
        PlayDrum();
        MoveArrowToNextLetter();

        if (letterIndex >= (currentWord.Length - 1))
        {
            letterIndex = 0;
            ChangeWord();

            pressedChar = ' ';
            pressed = false;

            ResetCurrentChar();

            return;
        }

        pressedChar = ' ';
        pressed = false;
        
        ShowLetterToType();
        
        startTime = Time.time;
        
        letterIndex++;

        _healthBar.DecreaseHealth(); // tylko dla testu tutaj odpalam, bo nie wiem gdzie
    }

    private void ResetCurrentChar()
    {
        currentChar = ' ';
        letterObject.text = currentChar.ToString();
    }

    private void ShowLetterToType()
    {
        currentChar = currentWord[letterIndex];
        letterObject.text = currentChar.ToString();
    }

    private void MoveArrowToNextLetter()
    {
        arrow.position = new Vector2(arrow.position.x + 23f, arrow.position.y);
    }

    private void PlayDrum()
    {
        audioData.PlayOneShot(drum1);
    }

    private void AnimateCharactersMovement()
    {
        player.ChangeSprite();
        enemy.ChangeSprite();
    }

    private void LoadHealthBar()
    {
        _healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar.IHealthBar>();
    }
    
    private void Update()
    {
        CheckForLetterInTime();
        Debug.Log(gameState);
        // Debug.Log(succesfulButtonPresses);
        // Debug.Log(currentWord.Length);
        //Debug.Log(dice.Length);
    }
}
