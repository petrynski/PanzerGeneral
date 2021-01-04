using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour
{
    [SerializeField]
    private UI_Shop _uiShop;
    private bool xd = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnMouseDown()
    {
        xd = !xd;
        _uiShop.SetVisible(xd);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
