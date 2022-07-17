using GameScore;
using UnityEngine.SceneManagement;

namespace ScoreScreen
{
    public static class ScoreScreenEventsHandler
    {
        public static void HandleGameOverEvent()
        {
            GameManager gameManager = GameManager.Instance;
            gameManager.StopCoroutine("DetermineOutcome");
            SceneManager.LoadScene("ScoreScreen");
        }
    }
}