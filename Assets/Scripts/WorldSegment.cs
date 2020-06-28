using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class WorldSegment : MonoBehaviour {

    public Tilemap enclosedTileMap;

    public int segmentYEnd;

    public float difficultyMinimum = 1;
    public float difficultyMaximum = 2;
    public int rollChance = 1;
    public string sequenceName;
    public int timesAllowedInSequence = 0;
    public SegmentationPipeType inputPipeType = SegmentationPipeType.Left | SegmentationPipeType.Middle | SegmentationPipeType.Right;
    public SegmentationPipeType outputPipeType = SegmentationPipeType.Left | SegmentationPipeType.Middle | SegmentationPipeType.Right;

    public bool swapNormalForReds = true;

    public TileBase[,] GetSegmentTiles(System.Random rand, TileBase swapBlock) {
        TileBase[,] toReturn = new TileBase[10,segmentYEnd];
        for (int x = 0; x < 10; x++) {
            for (int y = 0; y < segmentYEnd; y++)
            {
                TileBase baseT = enclosedTileMap.GetTile(new Vector3Int(x, y, 0));
                if (baseT is Tile && ((Tile)baseT).name == "BlockTile" && rand.Next(15, 80) <= (int)(PlayerBoxController.instance.difficulty * 10)) {
                    baseT = swapBlock;
                }
                toReturn[x, y] = baseT;
            }
        }
        return toReturn;
    }

    [Flags]
    public enum SegmentationPipeType {
        Left = 1,
        Right = 2,
        Middle = 4,
        LeftAndRight = Left | Right,
        LeftAndMiddle = Left | Middle,
        RightAndMiddle = Right | Middle,
        All = Left | Right | Middle
    }
}