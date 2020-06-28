using UnityEngine.UI;

public class InGameScreen : GUIWindow
{
    public Text scoreText;
    public Text goldCoinText;
    public Text silverCoinText;

    public void Update()
    {
        scoreText.text = "Score: " + ((int)PlayerBoxController.instance.currentScore) + "\nMeters: " + (int)((PlayerBoxController.instance.playerCube.position.y / 0.32f) + 5);
        goldCoinText.text = "x " + PlayerBoxController.currentPlayer.goldCoins;
        silverCoinText.text = "x " + PlayerBoxController.currentPlayer.silverCoins;
    }
}