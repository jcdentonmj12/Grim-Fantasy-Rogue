using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;



public class TileMapGenerator : MonoBehaviour
{
    public int mapWidth = 50;
    public int mapHeight = 50;
    public float scale = 3f; // Scale of the noise
    public static TileData[,] tileDataMatrix; // Matrix of TileData objects
    public float tileSize = 1f;

    public Tilemap waterTileMap;
    public Tilemap otherTileMap; 

    void Start()
    {
        Generate();
    }

    public TileData[,] Generate()
    {
        tileDataMatrix = new TileData[mapWidth, mapHeight];

        string pathToJson = "tilemap.json";

        if (File.Exists(pathToJson))
        {
            //LoadFromJson(); // Load from JSON if the file exists
            tileDataMatrix = SaveSystem.Load<TileData>(pathToJson).To2DArray(mapWidth, mapHeight);
        }
        else
        {
            // Generate the tile map if the file does not exist
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    float perlinValue = Mathf.PerlinNoise(x / scale, y / scale);
                    int tileType;
                    int motes = 5;
                    int height = 0;
                    bool canWalk;

                    if (perlinValue < 0.33f)
                    {
                        tileType = 1; // Water
                        canWalk = false;
                    }
                    else if (perlinValue < 0.66f)
                    {
                        tileType = 2; // Dirt
                        canWalk = true;
                    }
                    else
                    {
                        tileType = 3; // Stone
                        canWalk = false;
                    }


                    tileDataMatrix[x, y] = new TileData(tileType, motes, height, canWalk);
                }
            }
            //SaveToJson(); // Save to JSON
            SaveSystem.Save(tileDataMatrix.To1DArray(), pathToJson);
        }

        LoadTilemaps();
        return tileDataMatrix;
    }

    /// Draw tile methods

    public AnimatedTile[] waterTiles;
    public AnimatedTile[] dirtTiles;
    public AnimatedTile[] stoneTiles;

    void LoadTiles(ref AnimatedTile[] tiles, string basePath, int count)
    {
        tiles = new AnimatedTile[count];
        for (int i = 0; i < count; i++)
        {
            tiles[i] = Resources.Load<AnimatedTile>($"{basePath}{i + 1}");
        }
    }

    void LoadTilemaps()
    {
        LoadTiles(ref waterTiles, "Tiles/WaterTile", 1); // Assumes 1 water variations
        LoadTiles(ref dirtTiles, "Tiles/DirtTile", 2); // Assumes 2 dirt variations
        LoadTiles(ref stoneTiles, "Tiles/StoneTile", 2); // Assumes 2 Stone variation

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                int tileType = tileDataMatrix[x, y].Type;
                Vector3Int position = new Vector3Int(x, y, 0);

                AnimatedTile tileToPlace = ChooseTileByType(tileType, x, y);
                if (tileType == 1) // Water
                    waterTileMap.SetTile(position, tileToPlace);
                else
                    otherTileMap.SetTile(position, tileToPlace);
            }
        }
    }
    AnimatedTile ChooseTileByType(int tileType, int x, int y)
    {
        float variationValue = Mathf.PerlinNoise(x / scale, y / scale);
        AnimatedTile[] tiles;

        switch (tileType)
        {
            case 1:
                tiles = waterTiles;
                break;
            case 2:
                tiles = dirtTiles;
                break;
            case 3:
                tiles = stoneTiles;
                break;
            default:
                return null;
        }

        int index = Random.Range(0, tiles.Length);
        // Debugging information
        if (tileType == 3) // Only log for stone tiles
        {
            Debug.Log($"At coordinates (x: {x}, y: {y}), variationValue: {variationValue}, index: {index}");
        }

        return tiles[index];
    }


    /// Mutate tile methods
    // 1. Changing Particular Tile Fields
    public void ChangeTileData(int x, int y, int newType, int newMotes, int newHeight)
    {
        if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
        {
            tileDataMatrix[x, y].Type = newType;
            tileDataMatrix[x, y].Motes = newMotes;
            tileDataMatrix[x, y].Height = newHeight;
        }
    }
    // 2.) Debugging
    public int debugX;
    public int debugY;
    public int debugNewType;
    public int debugNewMotes;
    public int debugNewHeight;
    public void DebugApplyChanges()
    {
        ChangeTileData(debugX, debugY, debugNewType, debugNewMotes, debugNewHeight);
        LoadTilemaps();
        SaveSystem.Save(tileDataMatrix.To1DArray(), "tilemap.json");
    }


}
