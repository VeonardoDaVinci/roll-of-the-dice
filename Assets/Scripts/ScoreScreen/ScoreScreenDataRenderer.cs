using GameScore;
using TMPro;
using UnityEngine;

namespace ScoreScreen
{
    public class ScoreScreenDataRenderer
    {
        public void RenderScoreOnScreen(GameScoreValue gameScoreValue)
        {
            var textObject = GameObject.FindGameObjectWithTag("InvasionCount").GetComponent<TextMeshProUGUI>();
            var bpmObject = GameObject.FindGameObjectWithTag("BpmCount").GetComponent<TextMeshProUGUI>();
            Debug.Log(textObject);
            Debug.Log(gameScoreValue.invasions);
            textObject.text = gameScoreValue.invasions.ToString();
            bpmObject.text = gameScoreValue.bpm.ToString();
        }
    }
}