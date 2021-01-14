using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Town : MonoBehaviour
{
    [SerializeField]
    private UI_Shop uiShop;
    public Text nameText;
    public bool isGerman;
    public bool activePlayer;
    // Start is called before the first frame update
    void Start()
    {
        if (isGerman)
        {
            GameManager.germanTowns.Add(this);
        }
        else
        {
            GameManager.zsrrTowns.Add(this);
        }
    }

    private void OnMouseUp()
    {
        if(activePlayer)
        {
            uiShop.SetSelectedTown(this);
            uiShop.SetVisible(true);
        }

    }

    public void CloseShop()
    {
        uiShop.SetVisible(false);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void CaptureTown()
    {
        if (isGerman)
        {
            GameManager.zsrrTowns.Add(this);
            GameManager.germanTowns.Remove(this);
            nameText.color = GameManager.zsrrTowns[0].nameText.color;
            if(GameManager.germanTowns.Count == 0)
                Debug.Log("koniec gry wygrało ZSRR");
        }
        else
        {
            GameManager.germanTowns.Add(this);
            GameManager.zsrrTowns.Remove(this);
            nameText.color = GameManager.germanTowns[0].nameText.color;
            if(GameManager.germanTowns.Count == 0)
                Debug.Log("koniec gry wygrali Naziści");
        }
        isGerman = !isGerman;
        activePlayer = true;
    }
}
