using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

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
    private float[,] attackEffectivity;
    private static int[] rewardsTable = new int[4] {30, 210, 150, 90};
    private static int[] costTable = new int[4] {50, 350, 250, 150};
    public bool isGerman;

    private void Start()
    {
        SetUp();
    }

    public void SetUp()
    {
        switch (unitType)
        {
            case UnitType.infantry:
                maxHP = 10f;
                attackRange = 1;
                vision = 2;
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
                vision = 3;
                speed = 1;
                break;

            case UnitType.armoredCar:
                maxHP = 10f;
                attackRange = 1;
                vision = 3;
                speed = 3;
                break;
        }
        currentHP = maxHP;
        text.text = ((int)maxHP).ToString();

        attackEffectivity = new float[4, 4]
        {
            {0.2f, 0.1f, 0.6f, 0.2f}, {0.4f, 0.2f, 0.2f, 0.6f},
            {0.1f, 0.6f, 0.2f, 0.4f}, {0.6f, 0.1f, 0.2f, 0.2f}
        };

        if (isGerman)
        {
            GameManager.germanUnits.Add(this);
            text.color = Color.blue;
        }
        else
        {
            GameManager.zsrrUnits.Add(this);
            text.color = Color.red;
        }
    }

    internal void Attack(Unit attackedUnit, bool attackedFirst)
    {
        if (Mathf.Abs(attackedUnit.transform.position.x - transform.position.x) <= attackRange &&
            Mathf.Abs(attackedUnit.transform.position.y - transform.position.y) <= attackRange)
        {
            attackedUnit.currentHP -= this.currentHP * attackEffectivity[(int)this.unitType, (int)attackedUnit.unitType];
            if (attackedUnit.currentHP <= 0)
            {
                if (isGerman)
                {
                    GameManager.cashP1 += rewardsTable[(int)attackedUnit.unitType];
                }
                else
                {
                    GameManager.cashP2 += rewardsTable[(int)attackedUnit.unitType];
                }
                attackedUnit.Die();
            }
            else if (attackedFirst)
            {
                attackedUnit.Attack(this,false);
            }
            attackedUnit.text.text = Math.Round(attackedUnit.currentHP,1).ToString();
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

    private void Die()
    {
        if (isGerman)
        {
            GameManager.germanUnits.Remove(this);
        }
        else
        {
            GameManager.zsrrUnits.Remove(this);
        }
        Destroy(this.gameObject);
    }

    public static int GetCost(UnitType unitType)
    {
        return costTable[(int) unitType];
    }

    public static Sprite GetSprite(UnitType unitType, bool isGerman)
    {
        if (isGerman)
            switch ((int)unitType)
            {   
                default:
                case 0: return Resources.Load<Sprite>("Sprites/NiemieckaPiech");
                case 1: return Resources.Load<Sprite>("Sprites/NiemieckiCzolg");
                case 2: return Resources.Load<Sprite>("Sprites/NiemieckiPczolg");
                case 3: return Resources.Load<Sprite>("Sprites/NiemieckaPpiech");
            }    
        else
            switch ((int)unitType)
            {
                default:
                case 0: return Resources.Load<Sprite>("Sprites/RuskiPiech");
                case 1: return Resources.Load<Sprite>("Sprites/RuskiCzolg");
                case 2: return Resources.Load<Sprite>("Sprites/RuskiPczolg");
                case 3: return Resources.Load<Sprite>("Sprites/RuskaPpiech");
            }
    }

}