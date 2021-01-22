using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private bool isExitPopupVisible = false;
    public Canvas popUp;
    public Canvas germanWins;
    public Canvas ZsrrWins;
    public Text playerText;
    public Text cashText;
    public Tilemap fogP1, fogP2;
    public static bool isPlayerOneTurn;
    public static List<Unit> germanUnits = new List<Unit>();
    public static List<Unit> zsrrUnits = new List<Unit>();
    public static List<Town> germanTowns = new List<Town>();
    public static List<Town> zsrrTowns = new List<Town>();
    public static int cashP1, cashP2;
    public UI_Shop uiShop;

    void Start()
    {
        cashP1 = cashP2 = 250;
        isPlayerOneTurn = true;
        playerText.text = "Tura gracza 1";
        cashText.text = "Piniążki: " + cashP1.ToString();
        TilemapRenderer tr = fogP2.GetComponent<TilemapRenderer>();
        tr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

        foreach (var unit in zsrrUnits)
            unit.Disable();
    }

    private void Update()
    {
        if (isPlayerOneTurn)
            cashText.text = "Money: " + cashP1.ToString();

        else
            cashText.text = "Money: " + cashP2.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
            ExitPopUp();

        if (germanTowns.Count == 0)
            ZsrrWins.gameObject.SetActive(true);

        if (zsrrTowns.Count == 0)
            germanWins.gameObject.SetActive(true);
    }

    private static void SwitchPlayer(List<Unit> currentlyActiveUnits, List<Unit> toBeActiveUnits, List<Town> currentlyActiveTowns, List<Town> toBeActiveTowns)
    {
        foreach (var unit in currentlyActiveUnits)
            unit.Disable();
        foreach (var unit in toBeActiveUnits)
            unit.Enable();
        Vector3 camerapos = toBeActiveTowns[0].transform.position;
        camerapos.z = -1.54f;
        CameraController.thisCamera.transform.position = camerapos;
    }

    public void EndTurn()
    {
        isPlayerOneTurn = !isPlayerOneTurn;
        if (isPlayerOneTurn)
        {
            cashP1 += 10;
            cashP2 += 10;
            SwitchPlayer(zsrrUnits, germanUnits, zsrrTowns, germanTowns);
            playerText.text = "Tura gracza 1";
            TilemapRenderer tr = fogP2.GetComponent<TilemapRenderer>();
            tr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            tr = fogP1.GetComponent<TilemapRenderer>();
            tr.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }

        else
        {
            SwitchPlayer(germanUnits, zsrrUnits, germanTowns, zsrrTowns);
            playerText.text = "Tura gracza 2";
            TilemapRenderer tr = fogP1.GetComponent<TilemapRenderer>();
            tr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            tr = fogP2.GetComponent<TilemapRenderer>();
            tr.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }
        uiShop.ChangeNation(isPlayerOneTurn);
        uiShop.SetVisible(false);
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }

    public void ExitPopUp()
    {
        isExitPopupVisible = !isExitPopupVisible;
        popUp.gameObject.SetActive(isExitPopupVisible);
    }

}
