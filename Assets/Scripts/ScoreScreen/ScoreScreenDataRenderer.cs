using GameScore;
using TMPro;
using UnityEngine;

namespace ScoreScreen
{
    public class ScoreScreenDataRenderer
    {
        public void RenderScoreOnScreen(GameScoreValue gameScoreValue)
        {
            var textObject = GameObject.FindGameObjectWithTag("ScoreTextBox").GetComponent<TextMeshPro>();

            textObject.text = gameScoreValue.Score.ToString();
        }
    }
}