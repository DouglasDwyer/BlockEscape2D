using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomizationSettings
{
    public int goldCoins;
    public int silverCoins;

    public string activeHat;
    public string activeSkin;
    public Color activeColor = new Color32(0, 255, 0, 255);
    public string activeTrail;

    public List<string> ownedHats = new List<string>();
    public List<string> ownedSkins = new List<string>();
    public List<Color> ownedColors = new List<Color>();
    public List<string> ownedTrails = new List<string>();

    public bool noAdvertisements = false;
    public DateTime timeUntilNextAd = DateTime.MinValue;

    public bool playerSignedInAlready = false;
    public bool playerSuggestedNameAlready = false;

    public bool receivedCoinsAlready = false;

    public int numberOfTimesOpened = 0;
}
