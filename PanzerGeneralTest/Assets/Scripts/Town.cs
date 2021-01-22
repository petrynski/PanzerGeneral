using UnityEngine;
using UnityEngine.UI;


public class Town : MonoBehaviour
{
    [SerializeField]
    private UI_Shop uiShop;
    public Text nameText;
    public bool isGerman;

    void Start()
    {
        new WaitForSeconds(1);
        if (isGerman)
            GameManager.germanTowns.Add(this);

        else
            GameManager.zsrrTowns.Add(this);
    }

    private void OnMouseUp()
    {
        if (GameManager.isPlayerOneTurn == isGerman)
        {
            uiShop.SetSelectedTown(this);
            uiShop.SetVisible(true);
        }
    }

    public void CloseShop()
    {
        uiShop.SetVisible(false);
    }

    public void CaptureTown()
    {
        if (isGerman)
        {
            GameManager.zsrrTowns.Add(this);
            GameManager.germanTowns.Remove(this);
            nameText.color = GameManager.zsrrTowns[0].nameText.color;
        }

        else
        {
            GameManager.germanTowns.Add(this);
            GameManager.zsrrTowns.Remove(this);
            nameText.color = GameManager.germanTowns[0].nameText.color;
        }

        isGerman = !isGerman;
    }
}
