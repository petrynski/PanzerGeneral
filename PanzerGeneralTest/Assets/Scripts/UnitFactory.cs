using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    public GameObject germanUnit;
    public GameObject zsrrUnit;

    public void generateUnit(UnitType unitType, bool isGerman, Vector3 startPosition)
    {
        Debug.Log("Twoja stara jeździ na żółwiu");
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
