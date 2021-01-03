using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;


public class GameManager : MonoBehaviour
{
    bool isPlayerOneTurn;
    public Text text;
    public Tilemap fogP1, fogP2;
    public List<Unit> germanUnits;
    public List<Unit> zsrrUnits;
    // Start is called before the first frame update
    void Start()
    {
        isPlayerOneTurn = true;
        text.text = "Tura gracza 1";
        TilemapRenderer tr = fogP2.GetComponent<TilemapRenderer>();
        tr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        foreach(var unit in zsrrUnits)
        {
            unit.unitPhase = ActivityPhase.noOperation;
            unit.GetUMC().SetHasMoved(true);
            unit.GetUMC().SetActivePlayer(false);
        }
    }

    private static void SwitchPlayer(List<Unit> currentlyActive, List<Unit> nextActive)
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
    }

    public void EndTurn()
    {
        isPlayerOneTurn = !isPlayerOneTurn;
        if (isPlayerOneTurn)
        {   
            SwitchPlayer(zsrrUnits,germanUnits);
            text.text = "Tura gracza 1";
            TilemapRenderer tr = fogP2.GetComponent<TilemapRenderer>();
            tr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            tr = fogP1.GetComponent<TilemapRenderer>();
            tr.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }
        else
        {
            SwitchPlayer(germanUnits,zsrrUnits);
            text.text = "Tura gracza 2";
            TilemapRenderer tr = fogP1.GetComponent<TilemapRenderer>();
            tr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            tr = fogP2.GetComponent<TilemapRenderer>();
            tr.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }
    }
}
