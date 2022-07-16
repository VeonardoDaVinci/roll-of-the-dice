using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScoreScreen
{
    public class ScoreScreenEventsHandler
    {
        public static void handleGameOverEvent()
        {
            SceneManager.LoadScene("ScoreScreen");
        }
    }
}