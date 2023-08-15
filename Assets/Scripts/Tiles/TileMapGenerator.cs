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
        mapWidth = TileWorld.mapWidth;
        mapHeight = TileWorld.mapHeight;
        Generate();
    }

    public TileData[,] Generate()
    {

        TileWorld.tileDataMatrix = new TileData[mapWidth, mapHeight];
        string pathToJson = "tilemap.json";

        if (!File.Exists(pathToJson))
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    float perlinValue = Mathf.PerlinNoise(x / scale, y / scale);
                    int tileType;
                    int motes = 5;
                    int height = Mathf.FloorToInt((perlinValue - 0.5f) * 10); // Generating height between -5 to 5

                    if (perlinValue < 0.5f)
                    {
                        tileType = 2; // Dirt
                    }
                    else
                    {
                        tileType = 3; // Stone
                    }

                    TileWorld.tileDataMatrix[x, y] = new TileData(tileType, motes, height);
                }
            }
            SaveSystem.Save(TileWorld.tileDataMatrix.To1DArray(), pathToJson);
        }
        else
        {
            TileWorld.tileDataMatrix = SaveSystem.Load<TileData>(pathToJson).To2DArray(mapWidth, mapHeight);
        }

        LoadTilemaps();
        return TileWorld.tileDataMatrix;
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
                int tileType = TileWorld.tileDataMatrix[x, y].Type;
                Vector3Int position = new Vector3Int(x, y, 0);

                AnimatedTile tileToPlace = ChooseTileByType(tileType, x, y);
                if (tileType == 1) // Water
                    waterTileMap.SetTile(position, tileToPlace);
                else
                    otherTileMap.SetTile(position, tileToPlace);
            }
        }
        UpdateTileColors();
    }

    public void UpdateTileColors()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                float colorValue = Mathf.InverseLerp(-5, 5, TileWorld.tileDataMatrix[x, y].Height) + .1f;
                otherTileMap.SetColor(position, Color.Lerp(Color.black, Color.white, colorValue));
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
            TileWorld.tileDataMatrix[x, y].Type = newType;
            TileWorld.tileDataMatrix[x, y].Motes = newMotes;
            TileWorld.tileDataMatrix[x, y].Height = newHeight;

            Vector3Int position = new Vector3Int(x, y, 0);
            AnimatedTile tileToPlace = ChooseTileByType(newType, x, y);
            if (newType == 1) // Water
                waterTileMap.SetTile(position, tileToPlace);
            else
                otherTileMap.SetTile(position, tileToPlace);

            // Update colors if height has changed
            UpdateTileColors();
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
        SaveSystem.Save(TileWorld.tileDataMatrix.To1DArray(), "tilemap.json");
    }


}
