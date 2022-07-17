using System.Collections;
using System.Collections.Generic;
using GameScore;
using ScoreScreen;
using TMPro;
using UnityEngine;

public class ScoreScreenMainScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var scoreRenderer = new ScoreScreenDataRenderer();
        var gameManager = GameManager.Instance;    
        
        scoreRenderer.RenderScoreOnScreen(gameManager.GameScoreValue());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
