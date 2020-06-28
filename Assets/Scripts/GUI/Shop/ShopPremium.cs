using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class ShopPremium : Shop {

    public Button purchaseNoAds;

    public new void Awake()
    {
        purchaseNoAds.gameObject.SetActive(!PlayerBoxController.currentPlayer.noAdvertisements);
        base.Awake();
    }

    public new void Update()
    {
        base.Update();
        purchaseNoAds.gameObject.SetActive(!PlayerBoxController.currentPlayer.noAdvertisements);
    }

    public void BuyCoins(Product bought)
    {
        foreach (ProductCatalogItem def in ProductCatalog.LoadDefaultCatalog().allProducts) {
            if(def.id == bought.definition.id && def.Payouts.Count > 0 && def.Payouts[0].subtype == "Silver")
            {
                PlayerBoxController.currentPlayer.silverCoins += (int)def.Payouts[0].quantity;
                return;
            }
        }
    }

    public void PurchaseFailure(Product attempted, PurchaseFailureReason reason)
    {
        if(reason != PurchaseFailureReason.UserCancelled)
        {
            GUIManagement.LoadGUISetupAdditive("Purchases/PurchaseError");
            Debug.LogError(reason);
        }
    }

    public void RemoveAdvertisements()
    {
        GUIManagement.LoadGUISetupAdditive("Purchases/NoAds");
        PlayerBoxController.currentPlayer.noAdvertisements = true;
        AdvertisementManager.instance.HideBanner();
    }

    public void RemoveAdvertisementFailure(Product attempted, PurchaseFailureReason reason)
    {
        if(reason == PurchaseFailureReason.DuplicateTransaction)
        {
            RemoveAdvertisements();
        }
        else if (reason != PurchaseFailureReason.UserCancelled)
        {
            GUIManagement.LoadGUISetupAdditive("Purchases/PurchaseError");
        }
    }
}
