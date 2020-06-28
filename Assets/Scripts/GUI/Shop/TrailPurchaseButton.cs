using System;
using UnityEngine;
using UnityEngine.UI;

public class TrailPurchaseButton : ShopButton
{
    public string displayName;
    public Text displayText;
    public Image displayImage;
    public Outline displayOutline;

    public void Awake()
    {
        Update();
    }

    public void Update()
    {
        if(IsPurchased())
        {
            displayText.text = displayName + "\n(Owned)";
            displayImage.color = new Color32(187, 0, 253, 255);
        }
        else if(typeNecessary == CoinType.Silver)
        {
            displayImage.color = new Color32(0, 169, 253, 255);
        }
        if(PlayerBoxController.currentPlayer.activeTrail == purchaseName)
        {
            displayOutline.effectColor = Color.white;
        }
        else
        {
            displayOutline.effectColor = Color.black;
        }
    }

    public override bool IsPurchased()
    {
        return PlayerBoxController.currentPlayer.ownedTrails.Contains(purchaseName);
    }

    public override bool PurchaseItem()
    {
        if(typeNecessary == CoinType.Gold)
        {
            if(PlayerBoxController.currentPlayer.goldCoins >= coinsNecessary)
            {
                PlayerBoxController.currentPlayer.goldCoins -= coinsNecessary;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (PlayerBoxController.currentPlayer.silverCoins >= coinsNecessary)
            {
                PlayerBoxController.currentPlayer.silverCoins -= coinsNecessary;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public override void ItemPurchased()
    {
        if (!PlayerBoxController.currentPlayer.ownedTrails.Contains(purchaseName)) { PlayerBoxController.currentPlayer.ownedTrails.Add(purchaseName); }
        PlayerBoxController.currentPlayer.activeTrail = purchaseName;
        PlayerBoxController.instance.DrawPlayerCube();
    }
}