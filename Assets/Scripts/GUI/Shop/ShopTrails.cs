using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class ShopTrails : Shop
{
    public Outline noEffectOutline;

    public new void Update()
    {
        if(string.IsNullOrEmpty(PlayerBoxController.currentPlayer.activeTrail)) { noEffectOutline.effectColor = Color.white; }
        else { noEffectOutline.effectColor = Color.black; }
        base.Update();
    }

    public void SetEffect(string productName)
    {
        PlayerBoxController.currentPlayer.activeTrail = productName;
        PlayerBoxController.instance.DrawPlayerCube();
    }

    public void ClearEffects()
    {
        PlayerBoxController.currentPlayer.ownedTrails.Clear();
        PlayerBoxController.currentPlayer.ownedHats.Clear();
        PlayerBoxController.currentPlayer.ownedColors.Clear();
        PlayerBoxController.currentPlayer.ownedSkins.Clear();
        PlayerBoxController.currentPlayer.noAdvertisements = false;
        PlayerBoxController.currentPlayer.playerSignedInAlready = false;
    }
}
