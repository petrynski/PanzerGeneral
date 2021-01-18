using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayButton : MonoBehaviour
{
    public void LaunchGame()
    {
        SceneManager.LoadScene("Scenes/Map1");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LaunchTutorial()
    {
        SceneManager.LoadScene("Scenes/Tutorial");
    }
}