using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShopButton : MonoBehaviour
{
    public Button innerButton;
    public string purchaseName;
    public int coinsNecessary;
    public CoinType typeNecessary;

    public void Start()
    {
        innerButton = GetComponent<Button>();
        innerButton.onClick.AddListener(() => {
            if (IsPurchased())
            {
                ItemPurchased();
            }
            else
            {
                if (PurchaseItem())
                {
                    ItemPurchased();
                }
                else
                {
                    NotEnoughCoins();
                }
            }
        });
    }

    public virtual bool IsPurchased()
    {
        return false;
    }

    public virtual bool PurchaseItem()
    {
        return false;
    }

    public virtual void NotEnoughCoins()
    {
        if(typeNecessary == CoinType.Gold)
        {
            if(coinsNecessary - PlayerBoxController.currentPlayer.goldCoins <= PlayerBoxController.currentPlayer.silverCoins * 21)
            {
                ((TooPoorScreen)GUIManagement.LoadGUISetupAdditive("Purchases/TooPoor")).Setup(purchaseName, coinsNecessary - PlayerBoxController.currentPlayer.goldCoins, () => { PurchaseItem(); ItemPurchased(); });
            }
            else
            {
                GUIManagement.LoadGUISetupAdditive("Purchases/NeedMoreSilver");
            }
        }
        else
        {
            GUIManagement.LoadGUISetupAdditive("Purchases/BuyMoreSilver");
        }
    }

    public virtual void ItemPurchased()
    {

    }
}