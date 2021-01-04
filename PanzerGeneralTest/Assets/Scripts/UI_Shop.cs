using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;

    private void Start()
    {
        CreateItemButton(Unit.GetSprite(UnitType.infantry),"Piechota", Unit.GetCost(UnitType.infantry), 0);
        CreateItemButton(Unit.GetSprite(UnitType.tank),"Czołg", Unit.GetCost(UnitType.tank), 1);
        CreateItemButton(Unit.GetSprite(UnitType.antiTank),"Anty Czoug", Unit.GetCost(UnitType.antiTank), 2);
        CreateItemButton(Unit.GetSprite(UnitType.armoredCar),"Anty Piechota", Unit.GetCost(UnitType.armoredCar), 3);
        SetVisible(false);
    }

    private void Awake()
    {
        container = transform.Find("Container");
        shopItemTemplate = container.Find("ShopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    private void CreateItemButton(Sprite itemSprite, string itemName, int itemCost, int positionIndex)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        shopItemTransform.gameObject.SetActive(true);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        float shopItemHeight = 50f;
        shopItemRectTransform.anchoredPosition = new Vector2(0, -(shopItemHeight + 5) * positionIndex);

        shopItemTransform.Find("Unit").GetComponent<Text>().text = itemName;
        shopItemTransform.Find("UnitCost").GetComponent<Text>().text = itemCost.ToString();

        shopItemTransform.Find("Image").GetComponent<Image>().sprite = itemSprite;
    }
}
