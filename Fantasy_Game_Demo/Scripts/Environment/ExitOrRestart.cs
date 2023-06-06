using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitOrRestart : MonoBehaviour
{
    public void RestartScene()
    {
        // Get the index of the current scene
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reload the current scene
        SceneManager.LoadScene(sceneIndex);
    }

    public void ExitGame()
    {
        // Quit the application
        Application.Quit();
    }
}
