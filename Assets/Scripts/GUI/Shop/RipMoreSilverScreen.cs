using System;
using UnityEngine;

public class RipMoreSilverScreen : GUIWindow
{
    public void OpenShop()
    {
        TitleScreen.loadShopOnStart = true;
        new LoadMainScene().LoadScene();
    }
}