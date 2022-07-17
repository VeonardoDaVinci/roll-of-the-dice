using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private GameObject gameManager;

    public void ExitGame()
    {
        Application.Quit();
    }
    
    public void GoToInstruction()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ReturnToGame()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        Destroy(gameManager);
        SceneManager.LoadScene("Game");
    }
}
