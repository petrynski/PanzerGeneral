using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;


public class GameManager : MonoBehaviour
{
    bool isPlayerOneTurn;
    public Text text;
    public Tilemap fogP1, fogP2;

    // Start is called before the first frame update
    void Start()
    {
        isPlayerOneTurn = true;
        text.text = "Tura gracza 1";
        TilemapRenderer tr = fogP2.GetComponent<TilemapRenderer>();
        tr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }

    public void EndTurn()
    {
        isPlayerOneTurn = !isPlayerOneTurn;
        if (isPlayerOneTurn)
        {
            text.text = "Tura gracza 1";
            TilemapRenderer tr = fogP2.GetComponent<TilemapRenderer>();
            tr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            tr = fogP1.GetComponent<TilemapRenderer>();
            tr.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }
        else
        {
            text.text = "Tura gracza 2";
            TilemapRenderer tr = fogP1.GetComponent<TilemapRenderer>();
            tr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            tr = fogP2.GetComponent<TilemapRenderer>();
            tr.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }
        foreach(Unit unit in FindObjectsOfType<Unit>())
        {
            unit.unitPhase = ActivityPhase.moveOrReplenish;
            unit.GetUMC().SetHasMoved(false);
        }

    }
}
