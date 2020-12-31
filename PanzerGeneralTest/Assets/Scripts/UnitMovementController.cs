using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitMovementController : MonoBehaviour
{
    private Vector2 movementInput;
    [SerializeField]
    public Tilemap fogOfWar;
    public Tilemap rangeMap;
    [SerializeField]
    public TileBase highlightTile;
    public LayerMask colliders;
    bool hasMoved;
    public int vision = 3;
    public int speed = 2;
    public float xOffset = 0.0f;
    public float yOffset = 0.0f;

    // Update is called once per frame
    void Update()
    {
        UpdateFogOfWar();
        if(movementInput.x == 0 && movementInput.y == 0)
        {
            hasMoved = false;
        }
        else if((movementInput.x != 0 || movementInput.y != 0) && !hasMoved)
        {
            hasMoved = true;
            GetMovementDirection();
        }
    }

    public void GetMovementDirection()
    {
        Vector3Int destinationUnitTile = fogOfWar.WorldToCell(movementInput);
        Vector3Int currentUnitTile = fogOfWar.WorldToCell(transform.position);
        print("dest: " + movementInput.ToString());
        print("curr: " + transform.position.ToString());
        print("*******************************************");
        Vector3 endPosition;
        print("dest: " + destinationUnitTile.ToString());
        print("curr: " + currentUnitTile.ToString());
        if (Physics2D.OverlapCircle(movementInput, 0.2f, colliders))
            Debug.Log("Collider blocked");
        else
        {
            if (Mathf.Abs(destinationUnitTile.x - currentUnitTile.x) <= speed && Mathf.Abs(destinationUnitTile.y - currentUnitTile.y) <= speed)
            {
                ShowUnitRange(false);
                endPosition = fogOfWar.CellToWorld(destinationUnitTile);
                endPosition.x += xOffset;
                endPosition.y += yOffset;
                transform.position = endPosition;
            }
            else
                Debug.Log("Move outta range");
        }
        movementInput = new Vector2(0,0);
    }

    public void Move(Vector2 input)
    {
        movementInput = input;
    }


    void UpdateFogOfWar()
    {
        Vector3Int currentUnitTile = fogOfWar.WorldToCell(transform.position);

        for (int x = -vision; x <= vision; x++)
            for (int y = -vision; y <= vision; y++)
                fogOfWar.SetTile(currentUnitTile + new Vector3Int(x, y, 0), null);

    }

    public void ShowUnitRange(bool visible)
    {
        Vector3Int currentTile = rangeMap.WorldToCell(transform.position);
        for (int x = -speed; x <= speed; x++)
            for (int y = -speed; y <= speed; y++)
                if (visible)
                {
                    Debug.Log("Tiling range");
                    rangeMap.SetTile(currentTile + new Vector3Int(x, y, 0), highlightTile);
                }
                else
                    rangeMap.SetTile(currentTile + new Vector3Int(x, y, 0), null);
    }

}
