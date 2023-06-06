using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartScreen : MonoBehaviour
{
    public void playButton()
    {
        SceneManager.LoadScene("Beta_Scene"); // The main scene of the game is loaded and gameplay begins
    }

    public void settingsButton()
    {
        SceneManager.LoadScene("SettingsScene"); // The settings menu is displayed to the player
    }

    public void exitButton()
    {
        Application.Quit(); // The game will exit
    }
}
