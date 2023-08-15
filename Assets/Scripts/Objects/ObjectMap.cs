using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ObjectMap : MonoBehaviour
{
    public int width = 50;
    public int height = 50;
    public GameObj[,] objectMatrix;
    /*
    [System.Serializable]
    public struct GameObj
    {
        public string uniqueID;
        public Vector3 position;
        public int health;
    }
    */
    public void Save()
    {
        GameObj[] flatArray = new GameObj[width * height];
        int index = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                flatArray[index] = objectMatrix[x, y];
                index++;
            }
        }

        SaveSystem.Save(flatArray, "objects.json");
    }

    public void Load()
    {
        GameObj[] flatArray = SaveSystem.Load<GameObj>("objects.json");
        if (flatArray == null) return;

        int index = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                objectMatrix[x, y] = flatArray[index];
                index++;
            }
        }
    }
}

/*
public static class SaveSystem
{
    private static string pathToJson = "tilemap.json";

    public static void Save(TileData[,] tileDataMatrix, int mapWidth, int mapHeight)
    {
        TileData[] flatArray = new TileData[mapWidth * mapHeight];
        int index = 0;
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                flatArray[index] = tileDataMatrix[x, y];
                index++;
            }
        }

        TileDataList dataList = new TileDataList { TileDataArray = flatArray };
        string jsonData = JsonUtility.ToJson(dataList, true);

        File.WriteAllText(pathToJson, jsonData);
    }

    public static TileData[,] Load(int mapWidth, int mapHeight)
    {
        if (!File.Exists(pathToJson)) return null;

        string jsonData = File.ReadAllText(pathToJson);
        TileDataList dataList = JsonUtility.FromJson<TileDataList>(jsonData);

        TileData[,] tileDataMatrix = new TileData[mapWidth, mapHeight];
        int index = 0;
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                tileDataMatrix[x, y] = dataList.TileDataArray[index];
                index++;
            }
        }

        return tileDataMatrix;
    }
}
*/
