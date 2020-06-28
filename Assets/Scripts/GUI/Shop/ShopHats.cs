using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class ShopHats : Shop
{
    public Outline noEffectOutline;

    public new void Update()
    {
        if(string.IsNullOrEmpty(PlayerBoxController.currentPlayer.activeHat)) { noEffectOutline.effectColor = Color.white; }
        else { noEffectOutline.effectColor = Color.black; }
        base.Update();
    }

    public void SetEffect(string productName)
    {
        PlayerBoxController.currentPlayer.activeHat = productName;
        PlayerBoxController.instance.DrawPlayerCube();
    }

    public void ClearEffects()
    {
        PlayerBoxController.currentPlayer.ownedTrails.Clear();
        PlayerBoxController.currentPlayer.ownedHats.Clear();
        PlayerBoxController.currentPlayer.ownedColors.Clear();
        PlayerBoxController.currentPlayer.ownedSkins.Clear();
    }
}
