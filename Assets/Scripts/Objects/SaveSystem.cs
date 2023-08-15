using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static void Save<T>(T[] data, string filename)
    {
        string jsonData = JsonUtility.ToJson(new DataList<T> { DataArray = data }, true);
        File.WriteAllText(filename, jsonData);
    }

    public static T[] Load<T>(string filename)
    {
        if (!File.Exists(filename)) return null;

        string jsonData = File.ReadAllText(filename);
        return JsonUtility.FromJson<DataList<T>>(jsonData).DataArray;
    }

    [System.Serializable]
    private class DataList<T>
    {
        public T[] DataArray;
    }
}


