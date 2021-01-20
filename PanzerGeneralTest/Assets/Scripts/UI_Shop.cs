using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;
    private Transform[] items = new Transform[4];
    private Town selectedTown;
    public Text title;
    public UnitFactory unitFactory;


    private void Start()
    {
        items[0] = CreateItemButton("Piechota", Unit.GetCost(UnitType.infantry), 0, UnitType.infantry);
        items[1] = CreateItemButton("Czołg", Unit.GetCost(UnitType.tank), 1, UnitType.tank);
        items[2] = CreateItemButton("Anty Czoug", Unit.GetCost(UnitType.antiTank), 2, UnitType.antiTank);
        items[3] = CreateItemButton("Anty Piechota", Unit.GetCost(UnitType.armoredCar), 3, UnitType.armoredCar);
        
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

    public void SetSelectedTown(Town town)
    {
        title.text = town.nameText.text;
        selectedTown = town;
    }

    public void ChangeNation(bool isGerman)
    {
        UnitType i = 0;
        foreach (var item in items)
            item.Find("Image").GetComponent<Image>().sprite = Unit.GetSprite(i++, isGerman);
    }

    private Transform CreateItemButton(string itemName, int itemCost, int positionIndex, UnitType unitType)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        shopItemTransform.gameObject.SetActive(true);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        float shopItemHeight = 50f;
        shopItemRectTransform.anchoredPosition = new Vector2(0, -(shopItemHeight + 5) * positionIndex);

        shopItemTransform.Find("Unit").GetComponent<Text>().text = itemName;
        shopItemTransform.Find("UnitCost").GetComponent<Text>().text = itemCost.ToString();
        shopItemTransform.Find("Image").GetComponent<Image>().sprite = Unit.GetSprite(unitType, true);
        shopItemTransform.GetComponent<Button>().onClick.AddListener(
            () => { TryBuyUnit(unitType); }
        );

        return shopItemTransform;
    }

    public void TryBuyUnit(UnitType unitType)
    {
        if (GameManager.isPlayerOneTurn)
        {
            if (!(GameManager.cashP1 < Unit.GetCost(unitType)))
            {
                GameManager.cashP1 -= Unit.GetCost(unitType);
                unitFactory.GenerateUnit(unitType, GameManager.isPlayerOneTurn, selectedTown.transform.position);
                SetVisible(false);
            }
        }

        else
            if (!(GameManager.cashP2 < Unit.GetCost(unitType)))
        {
            GameManager.cashP2 -= Unit.GetCost(unitType);
            unitFactory.GenerateUnit(unitType, GameManager.isPlayerOneTurn, selectedTown.transform.position);
            SetVisible(false);
        }
    }
}
