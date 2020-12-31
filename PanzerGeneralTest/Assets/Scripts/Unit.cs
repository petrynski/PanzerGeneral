using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GameObject selectedGameObject;
    private UnitMovementController umc;

    private void Awake()
    {
        selectedGameObject = transform.Find("Selected").gameObject;
        SetSelectedVisible(false);
    }

    public void SetSelectedVisible(bool visible)
    {
        umc = this.GetComponent<UnitMovementController>();
        selectedGameObject.SetActive(visible);
        umc.ShowUnitRange(visible);
    }

    public void Move(Vector2 destination)
    {
        umc = this.GetComponent<UnitMovementController>();
        umc.Move(destination);
    }
}