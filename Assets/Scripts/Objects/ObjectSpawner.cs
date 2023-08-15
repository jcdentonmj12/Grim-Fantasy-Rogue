using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject floorPrefab, wallPrefab, doorPrefab;
    public int numberOfHouses;
    private ObjectMap objectMap;

    private void Start()
    {

    }

    public void Generate()
    {
        objectMap = GetComponent<ObjectMap>();
        objectMap.Load(); // Load from JSON
        SpawnHouses();
    }

    private void SpawnHouses()
    {
        for (int i = 0; i < numberOfHouses; i++)
        {
            int x = Random.Range(0, objectMap.width);
            int y = Random.Range(0, objectMap.height);

            // Place the floor, wall, and door objects based on the design
            Instantiate(floorPrefab, new Vector3(x, y, 0), Quaternion.identity);
            // Add walls and doors...

            // Add to object map
            int objHeight = TileMapGenerator.tileDataMatrix[x, y].Height;
            GameObj house = new GameObj { uniqueID = i.ToString(), position = new Vector2Int(x, y), height = objHeight, health = 100 };
            objectMap.objectMatrix[x, y] = house;
        }

        objectMap.Save(); // Save to JSON
    }
}

