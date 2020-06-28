using System;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : GUIWindow {

    public Text tauntText;
    public Text goldCoinText;
    public Text silverCoinText;
    public Text keepGoingText;

    public WorldSegment emptySegment;

    public void Start()
    {
        AdvertisementManager.instance.ShowInterstitial();
        tauntText.text = string.Format(tauntText.text, (int)(PlayerBoxController.instance.playerCube.position.y / 0.32f) + 5, (int)PlayerBoxController.instance.currentScore);
        goldCoinText.text = "x " + PlayerBoxController.currentPlayer.goldCoins;
        silverCoinText.text = "x " + PlayerBoxController.currentPlayer.silverCoins;
        keepGoingText.text = "x " + (Math.Pow(2, PlayerBoxController.instance.deathReviveTimes) * 10);
    }

    public void Continue()
    {
        if(PlayerBoxController.currentPlayer.silverCoins < Math.Pow(2, PlayerBoxController.instance.deathReviveTimes) * 10)
        {
            GUIManagement.LoadGUISetupSingular("Purchases/RipMoreSilver");
            return;
        }
        PlayerBoxController.currentPlayer.silverCoins -= (int)Math.Pow(2, PlayerBoxController.instance.deathReviveTimes) * 10;
        PlayerBoxController.instance.deathReviveTimes++;
        PlayerBoxController.instance.playerCube.isKinematic = false;
        PlayerBoxController.instance.playerCube.velocity = Vector2.zero;
        PlayerBoxController.instance.playerCube.angularVelocity = 0;
        PlayerBoxController.instance.playing = true;
        PlayerBoxController.instance.deathKiller.enabled = false;
        PlayerBoxController.instance.deathKiller.Reset();
        PlayerBoxController.instance.playerCube.transform.position = new Vector2(0, WorldGenerator.instance.GetWorldPositionAtTile(new Vector3Int(0, WorldGenerator.instance.nextYLevelToGenerate + 5, 0)).y);
        PlayerBoxController.instance.playerCube.position = new Vector2(0, WorldGenerator.instance.GetWorldPositionAtTile(new Vector3Int(0, WorldGenerator.instance.nextYLevelToGenerate + 5, 0)).y);
        WorldGenerator.instance.GenerateSegment(emptySegment);
        GUIManagement.LoadGUISetupSingular("InGame");
        PlayerBoxController.instance.DrawPlayerCube();
        PlayerBoxController.instance.playerCube.transform.localScale = new Vector3(0.7f, 0.7f, 1);
    }
}