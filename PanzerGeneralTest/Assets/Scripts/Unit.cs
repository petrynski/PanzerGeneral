using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public enum ActivityPhase : int
{
    moveOrReplenish = 0,
    attack = 1,
    noOperation = 2
}

public class Unit : MonoBehaviour
{
    private GameObject selectedGameObject;
    private UnitMovementController umc;
    public ActivityPhase unitPhase = 0;
    public int currentHP;
    public int maxHP;
    public int attackRange;
    public Text text;

    private void Start()
    {
        text.text = maxHP.ToString();
        currentHP = maxHP;
    }

    private void Awake()
    {
        selectedGameObject = transform.Find("Selected").gameObject;
        SetSelectedVisible(false);
    }

    public void SetSelectedVisible(bool visible)
    {
        umc = this.GetComponent<UnitMovementController>();
        selectedGameObject.SetActive(visible);
        if (unitPhase == ActivityPhase.moveOrReplenish)
            umc.ShowUnitRange(visible);
        else
            umc.ShowUnitRange(false);
    }

    public void Move(Vector2 destination)
    {
        umc = this.GetComponent<UnitMovementController>();
        umc.Move(destination);
        if (umc.GetHasMoved())
        {
            unitPhase = ActivityPhase.attack;
        }
    }

    public UnitMovementController GetUMC()
    {
        return umc;
    }
}