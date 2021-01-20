using System;
using UnityEngine;
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

// Klasa odpowiada za logikę działania jednostki. Tutaj zmienia się stan jednostki, następują interakcje między nimi i wydawane są polecenia poruszania się do unit movement controllera (UMC).
public class Unit : MonoBehaviour
{
    public UnitType unitType;
    private GameObject selectedGameObject;
    private UnitMovementController umc;
    public ActivityPhase unitPhase = 0;
    public float currentHP;
    public float maxHP;
    public int attackRange;
    public int vision;
    public int speed;
    public Text text;
    private static readonly float[,] attackEffectivity = new float[4, 4]
        {
            {0.2f, 0.1f, 0.6f, 0.2f}, {0.4f, 0.2f, 0.2f, 0.6f},
            {0.1f, 0.6f, 0.2f, 0.4f}, {0.6f, 0.1f, 0.2f, 0.2f}
        };
    private static readonly int[] rewardsTable = new int[4] {30, 210, 150, 90};
    private static readonly int[] costTable = new int[4] {50, 350, 250, 150};
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
                vision = 3;
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
                vision = 4;
                speed = 3;
                break;
        }
        currentHP = maxHP;
        text.text = ((int)maxHP).ToString();

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
        umc.vision = vision;
    }

    internal void Attack(Unit attackedUnit, bool attackedFirst)
    {
        if (umc.IsInRange(attackRange, attackedUnit.transform.position) && attackedUnit != this)
        {
            attackedUnit.currentHP -= this.currentHP * attackEffectivity[(int)this.unitType, (int)attackedUnit.unitType];
            if (attackedUnit.currentHP <= 0)
            {
                if (isGerman)
                    GameManager.cashP1 += rewardsTable[(int)attackedUnit.unitType];

                else
                    GameManager.cashP2 += rewardsTable[(int)attackedUnit.unitType];

                attackedUnit.Die();
            }
            else if (attackedFirst)
                attackedUnit.Attack(this,false);

            attackedUnit.text.text = Math.Round(attackedUnit.currentHP,1).ToString();
            unitPhase = ActivityPhase.noOperation;
            umc.ShowUnitRange(false, unitPhase, vision);
        }
    }

    private void Awake()
    {
        umc = GetComponent<UnitMovementController>();
        selectedGameObject = transform.Find("Selected").gameObject;
        SetSelectedVisible(false);
    }

    public void SetSelectedVisible(bool visible)
    {
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
        umc.Move(destination, speed);
        if (umc.GetHasMoved())
            unitPhase = ActivityPhase.attack;

    }

    private void Die()
    {
        if (isGerman)
            GameManager.germanUnits.Remove(this);

        else
            GameManager.zsrrUnits.Remove(this);

        Destroy(this.gameObject);
    }

    public void Disable()
    {
        unitPhase = ActivityPhase.noOperation;
        umc.SetHasMoved(true);
    }

    public void Enable()
    {
        unitPhase = ActivityPhase.moveOrReplenish;
        umc.SetHasMoved(false);
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