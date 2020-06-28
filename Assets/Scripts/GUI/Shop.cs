using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : GUIWindow
{
    public Text goldCoins;
    public Text silverCoins;

    public void Awake()
    {
        Update();
    }

    public void Update()
    {
        goldCoins.text = "x " + PlayerBoxController.currentPlayer.goldCoins;
        silverCoins.text = "x " + PlayerBoxController.currentPlayer.silverCoins;
    }
}