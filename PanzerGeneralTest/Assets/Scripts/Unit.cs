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

public enum UnitType : int
{
    infantry = 0,
    tank = 1,
    antiTank = 2,
    armoredCar = 3
}

public class Unit : MonoBehaviour
{
    public UnitType unitType;
    private GameObject selectedGameObject;
    private UnitMovementController umc;
    public ActivityPhase unitPhase = 0;
    public float currentHP;
    public float maxHP;
    public int attackRange;
    public Text text; 
    public int vision;
    public int speed;

    private void Start()
    {
        switch(unitType){
            case UnitType.infantry:
                maxHP = 10f;
                attackRange = 1;
                vision = 4;
                speed = 2;
                break;

            case UnitType.tank:
                maxHP = 10f;
                attackRange = 1;
                vision = 2;
                speed = 2;
                break;

            case UnitType.antiTank:
                maxHP = 10f;
                attackRange = 2;
                vision = 4;
                speed = 1;
                break;

            case UnitType.armoredCar:
                maxHP = 10f;
                attackRange = 1;
                vision = 4;
                speed = 3;
                break;
        }
        currentHP = maxHP;
        text.text = ((int)maxHP).ToString();
    }

    internal void attack(Unit attackedUnit)
    {
        if (Mathf.Abs(attackedUnit.transform.position.x - transform.position.x) <= attackRange &&
            Mathf.Abs(attackedUnit.transform.position.y - transform.position.y) <= attackRange)
        {
            attackedUnit.currentHP--;
            attackedUnit.text.text = attackedUnit.currentHP.ToString();
            unitPhase = ActivityPhase.noOperation;
            umc.ShowUnitRange(false, unitPhase, vision);
        }
        else
            Debug.Log("Outta range");
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
            umc.ShowUnitRange(visible, ActivityPhase.moveOrReplenish, speed);
        else if (unitPhase == ActivityPhase.attack)
            umc.ShowUnitRange(visible, ActivityPhase.attack, attackRange);
        else
            umc.ShowUnitRange(false, ActivityPhase.moveOrReplenish, vision);
    }

    public void Move(Vector2 destination)
    {
        umc = this.GetComponent<UnitMovementController>();
        umc.Move(destination, speed);
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