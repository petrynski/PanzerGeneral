using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    public GameObject germanUnit;
    public GameObject zsrrUnit;

    public void GenerateUnit(UnitType unitType, bool isGerman, Vector3 startPosition)
    {
        GameObject unitGO;

        if (isGerman)
            unitGO = Instantiate(germanUnit, startPosition, Quaternion.identity);

        else
            unitGO = Instantiate(zsrrUnit, startPosition, Quaternion.identity);

        Unit unit = unitGO.GetComponent<Unit>();
        unitGO.GetComponent<SpriteRenderer>().sprite = Unit.GetSprite(unitType, isGerman);
        unit.unitType = unitType;
        unit.unitPhase = ActivityPhase.noOperation;
        unit.isGerman = isGerman;
        unit.SetUp();
    }
}
