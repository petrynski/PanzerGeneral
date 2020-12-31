using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickController : MonoBehaviour
{
    private Unit selectedUnit = null;

       void OnMouseUp()
    {
        Collider2D collider = this.GetComponent<Collider2D>();
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
                selectedUnit.Move((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
