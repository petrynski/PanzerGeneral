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
}
