using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GoldCoinPurchaseButton : ShopButton
{
    public override bool PurchaseItem()
    {
        if (PlayerBoxController.currentPlayer.silverCoins >= coinsNecessary)
        {
            PlayerBoxController.currentPlayer.goldCoins += int.Parse(purchaseName);
            PlayerBoxController.currentPlayer.silverCoins -= coinsNecessary;
            return true;
        }
        return false;
    }
}