using System;
using UnityEngine;
using UnityEngine.Tilemaps;

// Klasa odpowiedzialna jest za współdziałanie instancji jednostki oraz siatki (zarówno mgły jak i mapy terenu). 
// Tutaj wykonuje się odsłanianie odkrytej mapy przez jednostkę oraz wizualne i logiczne przesuwanie jednostki po mapie.
public class UnitMovementController : MonoBehaviour
{
    private Vector2 movementInput;
    private Vector3 endPosition;
    [SerializeField]
    public Tilemap fogOfWar;
    public Tilemap rangeMap;
    [SerializeField]
    public TileBase highlightTile;
    public LayerMask colliders;
    private bool hasMoved;
    public int vision = 3;
    public float xOffset = 0.0f;
    public float yOffset = 0.0f;

    void Update()
    {
        UpdateFogOfWar();
        if (movementInput.x == 0 && movementInput.y == 0)
            if (hasMoved == true && transform.position != endPosition)
                transform.position = Vector3.MoveTowards(transform.position, endPosition, 0.02f);
    }

    private void Start()
    {
        endPosition = transform.position;
    }

    public void GetMovementDirection(int speed)
    {
        Vector3Int destinationUnitTile = fogOfWar.WorldToCell(movementInput);
        if (!Physics2D.OverlapCircle(movementInput, 0.1f, colliders))
            if (IsInRange(speed, movementInput))
            {
                hasMoved = true;
                endPosition = fogOfWar.CellToWorld(destinationUnitTile);
                endPosition.x += xOffset;
                endPosition.y += yOffset;
            }

        movementInput = new Vector2(0, 0);
        ShowUnitRange(false, ActivityPhase.moveOrReplenish, speed);
    }

    public void Move(Vector2 input, int speed)
    {
        movementInput = input;
        if ((movementInput.x != 0 || movementInput.y != 0) && !hasMoved)
            GetMovementDirection(speed);
    }

    void UpdateFogOfWar()
    {
        Vector3Int currentUnitTile = fogOfWar.WorldToCell(transform.position);

        if (currentUnitTile.y % 2 == 0)
            for (int y = -vision; y <= vision; y++)
                for (int x = -vision + (Math.Abs(y) / 2); x <= vision - ((Math.Abs(y) + 1) / 2); x++)
                    fogOfWar.SetTile(currentUnitTile + new Vector3Int(x, y, 0), null);
        else
            for (int y = -vision; y <= vision; y++)
                for (int x = -vision + ((Math.Abs(y) + 1) / 2); x <= vision - (Math.Abs(y) / 2); x++)
                    fogOfWar.SetTile(currentUnitTile + new Vector3Int(x, y, 0), null);
    }

    public void ShowUnitRange(bool visible, ActivityPhase ap, int range)
    {
        Vector3Int currentTile = rangeMap.WorldToCell(transform.position);
        if (currentTile.y % 2 == 0)
            for (int y = -range; y <= range; y++)
                for (int x = -range + (Math.Abs(y) / 2); x <= range - ((Math.Abs(y) + 1) / 2); x++)
                    if (visible)
                    {
                        if (ap == ActivityPhase.moveOrReplenish)
                        {
                            rangeMap.SetTile(currentTile + new Vector3Int(x, y, 0), highlightTile);
                            rangeMap.color = Color.white;
                        }
                        else if (ap == ActivityPhase.attack)
                        {
                            rangeMap.SetTile(currentTile + new Vector3Int(x, y, 0), highlightTile);
                            rangeMap.color = Color.red;
                        }
                    }
                    else
                        rangeMap.SetTile(currentTile + new Vector3Int(x, y, 0), null);
        else
            for (int y = -range; y <= range; y++)
                for (int x = -range + ((Math.Abs(y) + 1) / 2); x <= range - (Math.Abs(y) / 2); x++)
                    if (visible)
                    {
                        if (ap == ActivityPhase.moveOrReplenish)
                        {
                            rangeMap.SetTile(currentTile + new Vector3Int(x, y, 0), highlightTile);
                            rangeMap.color = Color.white;
                        }
                        else if (ap == ActivityPhase.attack)
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

    public bool IsInRange(int range, Vector3 destination)
    {
        Vector3Int destinationTile = fogOfWar.WorldToCell(destination);
        Vector3Int currentUnitTile = fogOfWar.WorldToCell(transform.position);
        if (!Physics2D.OverlapCircle(movementInput, 0.1f, colliders))
        {
            int x = destinationTile.x - currentUnitTile.x;
            int y = destinationTile.y - currentUnitTile.y;
            if (Math.Abs(y) <= range)
            {
                if (currentUnitTile.y % 2 == 0)
                {
                    if (x >= -range + (Math.Abs(y) / 2) && x <= range - ((Math.Abs(y) + 1) / 2))
                        return true;
                }
                else
                {
                    if (x >= -range + ((Math.Abs(y) + 1) / 2) && x <= range - (Math.Abs(y) / 2))
                        return true;
                }
            }
        }
        return false;
    }
}
