using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TileDataList
{
    public TileData[] TileDataArray; // Changed to a field, not a property
}

[System.Serializable]
public class TileData
{
    public int Type; // Tile type (1 for water, 2 for dirt, 3 for tree, etc.)
    public int Motes; // motes ranging from 1 to 100
    public int Height; // Height ranging from -100, to -2, to 2
    public bool CanWalk;

    public TileData(int type, int motes, int height, bool canWalk)
    {
        Type = type;
        Motes = motes;
        Height = height;
        CanWalk = canWalk;
    }
}
