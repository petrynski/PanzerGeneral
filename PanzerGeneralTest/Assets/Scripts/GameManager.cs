using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Tilemaps;


public class GameManager : MonoBehaviour
{
    bool isPlayerOneTurn;
    public Text playerText;
    public Text cashText;
    public Tilemap fogP1, fogP2;
    public static List<Unit> germanUnits = new List<Unit>();
    public static List<Unit> zsrrUnits = new List<Unit>();
    public static List<Town> germanTowns = new List<Town>();
    public static List<Town> zsrrTowns = new List<Town>();
    public static int cashP1, cashP2;
    [SerializeField]
    public UI_Shop uiShop;
    // Start is called before the first frame update
    void Start()
    {
        cashP1 = cashP2 = 250;
        isPlayerOneTurn = true;
        playerText.text = "Tura gracza 1";
        cashText.text = "Piniążki: " + cashP1.ToString();
        TilemapRenderer tr = fogP2.GetComponent<TilemapRenderer>();
        tr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        foreach(var unit in zsrrUnits)
        {
            unit.unitPhase = ActivityPhase.noOperation;
            unit.GetUMC().SetHasMoved(true);
            unit.GetUMC().SetActivePlayer(false);
        }
    }

    private void Update()
    {
        if (isPlayerOneTurn)
        {
            cashText.text = "Piniążki: " + cashP1.ToString();
        }
        else
        {
            cashText.text = "Piniążki: " + cashP2.ToString();
        }
    }

    private static void SwitchPlayer(List<Unit> currentlyActive, List<Unit> nextActive, List<Town> currActTowns, List<Town> nextActTowns)
    {
        foreach(var unit in currentlyActive)
        {
            unit.unitPhase = ActivityPhase.noOperation;
            unit.GetUMC().SetHasMoved(true);
            unit.GetUMC().SetActivePlayer(false);
            unit.GetUMC().Move(new Vector2(0,0), unit.speed);
        }
        foreach(var unit in nextActive)
        {
            unit.unitPhase = ActivityPhase.moveOrReplenish;
            unit.GetUMC().SetHasMoved(false);
            unit.GetUMC().SetActivePlayer(true);
        }
        foreach (var town in currActTowns)
            town.activePlayer = false;
        foreach (var town in nextActTowns)
            town.activePlayer = true;

    }

    public void EndTurn()
    {
        isPlayerOneTurn = !isPlayerOneTurn;
        if (isPlayerOneTurn)
        {
            cashP1 += 10;
            cashP2 += 10;
            SwitchPlayer(zsrrUnits,germanUnits, zsrrTowns, germanTowns);
            playerText.text = "Tura gracza 1";
            TilemapRenderer tr = fogP2.GetComponent<TilemapRenderer>();
            tr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            tr = fogP1.GetComponent<TilemapRenderer>();
            tr.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;

        }
        else
        {
            SwitchPlayer(germanUnits,zsrrUnits, germanTowns, zsrrTowns);
            playerText.text = "Tura gracza 2";
            TilemapRenderer tr = fogP1.GetComponent<TilemapRenderer>();
            tr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            tr = fogP2.GetComponent<TilemapRenderer>();
            tr.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;

        }
        uiShop.ChangeNation(isPlayerOneTurn);
        uiShop.SetVisible(false);
    }
}
