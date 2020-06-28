using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour {

    public static WorldGenerator instance;

    public Tilemap loadedTilemap;
    public WorldSegment[] allPossibleSegments;

    public int xOffset;
    public TileBase lethalBlock;
    public WorldSegment thousandMonument;

    public int bottomX;
    public int nextYLevelToGenerate;
    public int lastYRowCleared;
    public int seed;

    private System.Random random;
    private string lastSegment;
    private int timesMoreForRoll;
    private WorldSegment.SegmentationPipeType lastOutputPipeType = WorldSegment.SegmentationPipeType.All;

    public void Start()
    {
        instance = this;
        if (seed == 0)
        {
            seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }
        random = new System.Random(seed);

        if (allPossibleSegments.Length == 0) {
            allPossibleSegments = Resources.LoadAll<WorldSegment>("TileSegments");
        }
    }

    public string GetTileAtWorldPosition(Vector3 worldPos)
    {
        TileBase tBase = loadedTilemap.GetTile(loadedTilemap.WorldToCell(worldPos));
        if(tBase is Tile)
        {
            return ((Tile)tBase).name;
        }
        return null;
    }

    public Vector3 GetWorldPositionAtTile(Vector3Int tile)
    {
        return loadedTilemap.CellToWorld(tile);
    }

    public void Update() {
        if (nextYLevelToGenerate - 15 < loadedTilemap.WorldToCell(PlayerBoxController.instance.transform.localPosition).y) {
            GenerateNextSegment(PlayerBoxController.instance.difficulty);
        }
        while (lastYRowCleared + 12 < loadedTilemap.WorldToCell(PlayerBoxController.instance.transform.localPosition).y) {
            for (int x = 0; x < 10; x++) {
                loadedTilemap.SetTile(new Vector3Int(x + xOffset, lastYRowCleared, 0), null);
            }
            lastYRowCleared++;
        }
    }

    public void GenerateNextSegment(float difficulty) {
        List<WorldSegment> diffSegmentated = new List<WorldSegment>();
        foreach (WorldSegment segment in allPossibleSegments) {
            if (segment.difficultyMinimum <= difficulty && segment.difficultyMaximum >= difficulty) {
                if ((segment.inputPipeType & lastOutputPipeType) > 0 && (segment.sequenceName != lastSegment || timesMoreForRoll > 0))
                {
                    for (int time = 0; time < segment.rollChance; time++)
                    {
                        diffSegmentated.Add(segment);
                    }
                }
            }
        }
        if (diffSegmentated.Count == 0) { return; }
        WorldSegment chosenSegment = diffSegmentated[random.Next(0, diffSegmentated.Count)];
        if((int)((PlayerBoxController.instance.playerCube.position.y / 0.32f) + 5) > 1000 && thousandMonument != null) {
            chosenSegment = thousandMonument;
            thousandMonument = null;
        }
        GenerateSegment(chosenSegment);
        if (lastSegment == chosenSegment.sequenceName)
        {
            timesMoreForRoll--;
        }
        else
        {
            lastSegment = chosenSegment.sequenceName;
            if (chosenSegment.timesAllowedInSequence > 0)
            {
                timesMoreForRoll = chosenSegment.timesAllowedInSequence - 1;
            }
            else
            {
                timesMoreForRoll = int.MaxValue;
            }
        }
    }

    public void GenerateSegment(WorldSegment chosenSegment)
    {
        SetNextTileSegment(chosenSegment.GetSegmentTiles(random, lethalBlock));
        nextYLevelToGenerate += chosenSegment.segmentYEnd;
        lastOutputPipeType = chosenSegment.outputPipeType;
    }

    public void SetNextTileSegment(TileBase[,] tiles) {
        for (int x = 0; x < tiles.GetLength(0); x++) {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                loadedTilemap.SetTile(new Vector3Int(x + xOffset, y + nextYLevelToGenerate, 0), tiles[x, y]);
            }
        }
    }
}