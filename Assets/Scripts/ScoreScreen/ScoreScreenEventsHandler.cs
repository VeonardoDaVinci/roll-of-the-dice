using GameScore;
using UnityEngine.SceneManagement;

namespace ScoreScreen
{
    public static class ScoreScreenEventsHandler
    {
        public static void HandleGameOverEvent()
        {
            SceneManager.LoadScene("ScoreScreen");
        }
    }
}