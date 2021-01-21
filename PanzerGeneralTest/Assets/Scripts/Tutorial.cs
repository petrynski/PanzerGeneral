using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Klasa odpowiedzialna za logikę działania tutorialu w menu gry.
public class Tutorial : MonoBehaviour
{
    public Image currentImage;
    public Sprite[] slides;
    private int currentSlide = 0;

    void Start()
    {
        currentImage.sprite = slides[currentSlide];
    }

    public void changeImage(bool forward)
    {
        if (forward)
        {
            if (currentSlide != slides.Length - 1)
            {
                currentSlide++;
                currentImage.sprite = slides[currentSlide];
            }
        }
        else
        {
            if (currentSlide != 0)
            {
                currentSlide--;
                currentImage.sprite = slides[currentSlide];
            }
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }
}
