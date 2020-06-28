using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockCollisionDetection : MonoBehaviour {

    public new Rigidbody2D rigidbody2D;
    public Tilemap gameMap;

    void Update()
    {
        if(WorldGenerator.instance.GetTileAtWorldPosition(transform.position) == "GoldCoin")
        {
            gameMap.SetTile(gameMap.WorldToCell(transform.position), null);
            SoundManager.PlaySoundEffect("CoinGet");
            PlayerBoxController.currentPlayer.goldCoins += 1;
            PlayerBoxController.instance.distanceAwayFromLavaBonus += PlayerBoxController.instance.difficulty;
        }
        if (WorldGenerator.instance.GetTileAtWorldPosition(transform.position) == "SilverCoin")
        {
            gameMap.SetTile(gameMap.WorldToCell(transform.position), null);
            SoundManager.PlaySoundEffect("CoinGet");
            PlayerBoxController.currentPlayer.silverCoins += 1;
            PlayerBoxController.instance.distanceAwayFromLavaBonus += PlayerBoxController.instance.difficulty * 10;
        }
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.F4))
        {
            if(!System.IO.Directory.Exists(Application.dataPath + "/../screenshots"))
            {
                System.IO.Directory.CreateDirectory(Application.dataPath + "/../screenshots");
            }
            ScreenCapture.CaptureScreenshot(Application.dataPath + "/../screenshots/" + DateTime.Now.ToString().Replace('/', '.').Replace(' ', ',').Replace(':', '.') + ".png");
        }
#endif
    }

    void OnCollisionEnter2D(Collision2D colObj) {
        OnCollisionStay2D(colObj);
    }

    void OnCollisionStay2D(Collision2D colObj)
    {
        Vector2 hitPosition = Vector3.zero;
        foreach (ContactPoint2D hit in colObj.contacts)
        {
            hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
            hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
            Vector3Int tileCoord = gameMap.WorldToCell(hitPosition);
            TileBase tileB = gameMap.GetTile(tileCoord);
            if (tileB is Tile) {
                if (((Tile)tileB).name == "BlockRedTile")
                {
                    PlayerBoxController.instance.KillPlayer();
                }
                else if (((Tile)tileB).name == "BlockCrackedTile") {
                    gameMap.SetTile(tileCoord, null);
                    SoundManager.PlaySoundEffect("HitDisappear");
                }
            }
        }
    }
}