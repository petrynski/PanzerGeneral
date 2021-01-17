using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickController : MonoBehaviour
{
    private Unit selectedUnit = null;

    void OnMouseUp()
    {
        Collider2D collider = GetComponent<Collider2D>();
        selectedUnit = collider.GetComponent<Unit>();

        if (selectedUnit != null)
        {
            selectedUnit.SetSelectedVisible(true);
            Debug.Log(name + " selected");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedUnit != null)
                selectedUnit.SetSelectedVisible(false);
            
            selectedUnit = null;
        }

        if(Input.GetMouseButtonDown(1))
        {
            if (selectedUnit != null)
            {
                if (selectedUnit.unitPhase == ActivityPhase.moveOrReplenish)
                    selectedUnit.Move((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
                
                if (selectedUnit.unitPhase == ActivityPhase.attack)
                {
                    Unit attackedUnit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
                    if (hit.collider != null)
                    {
                        attackedUnit = hit.collider.GetComponent<Unit>();
                        if (attackedUnit != null)
                        { 
                            Debug.Log(selectedUnit.ToString() + " atakuje " + attackedUnit.ToString());
                            selectedUnit.Attack(attackedUnit, true);
                        }
                        //TODO przepiąć do UMC w fazie ruchu
                        else
                        {
                            Town capturedTown = hit.collider.GetComponent<Town>();
                            if (capturedTown != null)
                            {
                                if (selectedUnit.isGerman && !capturedTown.isGerman)
                                {
                                    Debug.Log(capturedTown.ToString() + " zostało podbite przez Nazistów");
                                    capturedTown.CaptureTown();
                                }
                                else if (!selectedUnit.isGerman && capturedTown.isGerman)
                                {
                                    Debug.Log(capturedTown.ToString() + " zostało podbite przez ZSRR");  
                                    capturedTown.CaptureTown();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
