using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class ShopColors : Shop
{
    public Outline noEffectOutline;
    public Color productColor;

    public new void Update()
    {
        if(PlayerBoxController.currentPlayer.activeColor == productColor) { noEffectOutline.effectColor = Color.white; }
        else { noEffectOutline.effectColor = Color.black; }
        base.Update();
    }

    public void SetEffect(Color productColor)
    {
        PlayerBoxController.currentPlayer.activeColor = productColor;
        PlayerBoxController.instance.DrawPlayerCube();
    }

    public void SetEffectDefault()
    {
        SetEffect(productColor);
    }

    public void ClearEffects()
    {
        PlayerBoxController.currentPlayer.ownedTrails.Clear();
        PlayerBoxController.currentPlayer.ownedHats.Clear();
        PlayerBoxController.currentPlayer.ownedColors.Clear();
        PlayerBoxController.currentPlayer.ownedSkins.Clear();
    }
}
