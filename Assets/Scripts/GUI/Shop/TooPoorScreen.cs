using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class TooPoorScreen : GUIWindow
{
    public Text buyingText;
    public Text costText;
    public int coinsNecessary;
    public UnityAction successfulCallback;

    public void Setup(string buying, int coins, UnityAction callback)
    {
        buyingText.text = "You don't have enough golden coins to buy " + buying + "!  Would you like to convert your silver coins to golden coins to purchase this item?";
        costText.text = "Buy " + coins + " golden coins for " + (coins / 21) + " silver ones?";
        coinsNecessary = coins;
        successfulCallback = callback;
    }

    public void PurchaseCoins()
    {
        PlayerBoxController.currentPlayer.goldCoins += coinsNecessary;
        PlayerBoxController.currentPlayer.silverCoins -= (coinsNecessary / 21);
        PlayerBoxController.currentPlayer.silverCoins = Math.Max(0, PlayerBoxController.currentPlayer.silverCoins);
        successfulCallback.Invoke();
        CloseWindow();
    }
}

public enum CoinType
{
    Gold,
    Silver
}