using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameObj
{
    public string uniqueID;
    public Vector2Int position;
    public int height;
    public int health;
    // Other attributes...
}

[System.Serializable]
public class NPC : GameObj
{
    public string name;
    // Other NPC-specific attributes...
}

