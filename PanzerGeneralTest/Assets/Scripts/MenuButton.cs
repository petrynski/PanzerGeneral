using UnityEngine;
using UnityEngine.SceneManagement;
// Klasa odpowiedzialna za działanie menu gry.
public class MenuButton : MonoBehaviour
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