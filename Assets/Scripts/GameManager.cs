using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using events;
using GameScore;
using HealthBar;
using ScoreScreen;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : SingletonPersistent<GameManager>
{
    public string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public TextMeshProUGUI letterObject;
    public TextMeshProUGUI typedLetterObject;
    public char currentChar;
    public int letterIndex = 0;
    public char pressedChar;

    //private Color green = new Color
    public TextMeshProUGUI messegeObject;
    private TextMeshProUGUI scoreObject;

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

    private string gameState = "rhythm";

    public int succesfulButtonPresses = 0;

    private GameObject[] dice;
    private int valueSum = 0;
    private GameObject[] enemyDice;
    private int enemyValueSum = 0;

    public int diceCount = 0;
    private IHealthBar _healthBar;
    
    public GameOverEvent GameOverEvent { get; private set; }

    private void Start()
    {
        InstantiateEvents();
        LoadHealthBar();
        HandleEvents();

        bpm = 50;
        
        LoadComponentsFromScreen();
        
        SetWord();
        
        StartRhythm();

        SetDiceAsNotActiveByDefault();

    }

    private void InstantiateEvents()
    {
        GameOverEvent = new GameOverEvent();
    }

    private void SetDiceAsNotActiveByDefault()
    {
        for (int die = 0; die < dice.Length; die++)
        {
            dice[die].SetActive(false);
        }
    }

    private void StartRhythm()
    {
        InvokeRepeating(nameof(ChangeLetter), 60f / bpm, 60f / bpm);
        InvokeRepeating(nameof(PlayRhythm), 30f / bpm, 30f / bpm);
    }

    private void StopRhythm()
    {
        CancelInvoke(nameof(ChangeLetter));
        CancelInvoke(nameof(PlayRhythm));
    }

    private void HandleEvents()
    {
        // uncomment to handle game over
        _healthBar.HealthReachedZeroEvent.AddListener(StopRhythm);
        _healthBar.HealthReachedZeroEvent.AddListener(GameOver);
        
        GameOverEvent.AddListener(ScoreScreenEventsHandler.HandleGameOverEvent);
    }

    private void GameOver()
    {
        GameOverEvent.Invoke();
    }

    public GameScoreValue GameScoreValue()
    {
        return new GameScoreValue(score);
    }

    private void LoadComponentsFromScreen()
    {
        audioData = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyBehaviour>();

        dice = GameObject.FindGameObjectsWithTag("Die");
        enemyDice = GameObject.FindGameObjectsWithTag("EnemyDie");
        Debug.Log(enemyDice.Length);
        letterObject = GameObject.FindGameObjectWithTag("Letter").GetComponent<TextMeshProUGUI>();
        typedLetterObject = GameObject.FindGameObjectWithTag("TypedLetter").GetComponent<TextMeshProUGUI>();
        wordObject = GameObject.FindGameObjectWithTag("Word").GetComponent<TextMeshProUGUI>();
        messegeObject = GameObject.FindGameObjectWithTag("Messege").GetComponent<TextMeshProUGUI>();
        scoreObject = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
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

    private void SetMessege(string messege)
    {
        messegeObject.text = messege;
    }

    private void ChangeWord()
    {
        if (succesfulButtonPresses == currentWord.Length-1)
        {
            Debug.Log(succesfulButtonPresses);
            Debug.Log(currentWord);
            score += 10;
            diceCount++;
            dice[diceCount - 1].SetActive(true);
            if (diceCount >= 6)
            {
                ChangeState();
            }
        }
        else
        {
            ChangeState();
        }
        //Debug.Log(succesfulButtoPresses);
        if (gameState == "rhythm")
        {
            SetWord();
        }
        else if (gameState == "hazard")
        {
            StopRhythm();
            valueSum = 0;
            enemyValueSum = 0;
            if (dice.Length > 0)
            {
                for (int a = 0; a <= dice.Length-1; a++)
                {
                    if (dice[a].activeSelf == true)
                    {
                        dice[a].GetComponent<DiceBehaviour>().SetRandomValue();
                        valueSum += dice[a].GetComponent<DiceBehaviour>().value;
                    }
                }
            }

            Debug.Log(enemyDice.Length);
            for(int b = 0; b <= enemyDice.Length-1; b++)
            {
                Debug.Log("in enemy dice loop");
                Debug.Log(enemyDice[b].GetComponent<DiceBehaviour>());
                enemyDice[b].GetComponent<DiceBehaviour>().SetRandomValue();
                enemyValueSum += enemyDice[b].GetComponent<DiceBehaviour>().value;

            }

            if (valueSum > enemyValueSum)
            {
                SetMessege("You've won. You keep your land");
                score += 100;
                StartCoroutine(RestartRound());
            }
            else if (valueSum < enemyValueSum)
            {
                SetMessege("I'm affraid you've lost a region");
                _healthBar.DecreaseHealth();
                score -= 10;
                StartCoroutine(RestartRound());
            }
            else
            {
                SetMessege("A draw! Well that's anti-climactic");
                score += 0;
                StartCoroutine(RestartRound());
            }

        }
    }

    private IEnumerator RestartRound()
    {
        yield return new WaitForSeconds(2.0f);

        diceCount = 0;

        SetMessege("");

        ChangeState();

        SetWord();

        StartRhythm();

        ResetDiceState();

        SetDiceAsNotActiveByDefault();
    }

    private void ChangeState()
    {
        if(gameState == "rhythm")
        {
            gameState = "hazard";
            return;
        }
        gameState = "rhythm";

    }

    private void PlayRhythm()
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
                //score += 100;
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

        /*_healthBar.DecreaseHealth();*/ // tylko dla testu tutaj odpalam, bo nie wiem gdzie
    }

    private void ResetDiceState()
    {
        if (dice.Length > 0)
        {
            Debug.Log(dice.Length);
            for (int a = 0; a <= dice.Length - 1; a++)
            {
                if (dice[a].activeSelf == true)
                {
                    dice[a].GetComponent<DiceBehaviour>().ResetState();
                }
            }
        }

        for (int b = 0; b <= enemyDice.Length - 1; b++)
        {
            enemyDice[b].GetComponent<DiceBehaviour>().ResetState();

        }
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

    private void ShowTypedLetter()
    {
        typedLetterObject.text = pressedChar.ToString();
        if (pressedChar == currentChar)
        {
            typedLetterObject.color = Color.green; 
        }
        else
        {
            typedLetterObject.color = Color.red;
        }
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
        ShowTypedLetter();
        scoreObject.text = score.ToString();
    }
}
