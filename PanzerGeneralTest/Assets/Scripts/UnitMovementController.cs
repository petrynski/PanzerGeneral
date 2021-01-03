using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitMovementController : MonoBehaviour
{
    private Vector2 movementInput;
    Vector3 endPosition;
    [SerializeField]
    public Tilemap fogOfWar;
    public Tilemap rangeMap;
    [SerializeField]
    public TileBase highlightTile;
    public LayerMask colliders;
    bool hasMoved;
    public int vision = 3;
    public float xOffset = 0.0f;
    public float yOffset = 0.0f;
    private bool activePlayer = true;

    // Update is called once per frame
    void Update()
    {
        UpdateFogOfWar();
        if (movementInput.x == 0 && movementInput.y == 0 && activePlayer)
        {
            if (hasMoved == true && transform.position != endPosition)
                transform.position = Vector3.MoveTowards(transform.position, endPosition, 0.02f);
        }
    }

    public void GetMovementDirection(int speed)
    {
        Vector3Int destinationUnitTile = fogOfWar.WorldToCell(movementInput);
        Vector3Int currentUnitTile = fogOfWar.WorldToCell(transform.position);
        /*print("dest: " + movementInput.ToString());
        print("curr: " + transform.position.ToString());
        print("*******************************************");
        print("dest: " + destinationUnitTile.ToString());
        print("curr: " + currentUnitTile.ToString());*/
        if (Physics2D.OverlapCircle(movementInput, 0.1f, colliders))
            Debug.Log("Collider blocked");
        else
        {
            if (Mathf.Abs(destinationUnitTile.x - currentUnitTile.x) <= speed && Mathf.Abs(destinationUnitTile.y - currentUnitTile.y) <= speed)
            {
                hasMoved = true;
                endPosition = fogOfWar.CellToWorld(destinationUnitTile);
                endPosition.x += xOffset;
                endPosition.y += yOffset;
            }
            else
                Debug.Log("Move outta range");
        }
        movementInput = new Vector2(0,0);

        ShowUnitRange(false, ActivityPhase.moveOrReplenish, speed);
    }

    public void Move(Vector2 input, int speed)
    {
        movementInput = input;
        if ((movementInput.x != 0 || movementInput.y != 0) && !hasMoved)
        {
            GetMovementDirection(speed);
        }
    }


    void UpdateFogOfWar()
    {
        Vector3Int currentUnitTile = fogOfWar.WorldToCell(transform.position);

        for (int x = -vision; x <= vision; x++)
            for (int y = -vision; y <= vision; y++)
                fogOfWar.SetTile(currentUnitTile + new Vector3Int(x, y, 0), null);

    }

    public void ShowUnitRange(bool visible, ActivityPhase ap, int range)
    {
        Vector3Int currentTile = rangeMap.WorldToCell(transform.position);
        for (int x = -range; x <= range; x++)
            for (int y = -range; y <= range; y++)
                if (visible)
                {
                    if (ap == ActivityPhase.moveOrReplenish)
                    {
                        rangeMap.SetTile(currentTile + new Vector3Int(x, y, 0), highlightTile);
                        rangeMap.color = Color.white;
                    }
                    else if(ap == ActivityPhase.attack)
                    {
                        rangeMap.SetTile(currentTile + new Vector3Int(x, y, 0), highlightTile);
                        rangeMap.color = Color.red;
                    }
                }
                else
                    rangeMap.SetTile(currentTile + new Vector3Int(x, y, 0), null);
    }

    internal bool IsAtPosition()
    {
        return transform.position == endPosition;
    }

    public bool GetHasMoved()
    {
        return hasMoved;
    }

    public void SetHasMoved(bool arg)
    {
        hasMoved = arg;
    }

    public void SetActivePlayer(bool arg)
    {
        activePlayer = arg;
    }
}
