using System.IO;
using System.IO.Enumeration;
using UnityEngine;

public static class InventorySaveSystem
{
    private const string FileName = "inventory.json";

    private static string SavePath => Path.Combine(Application.persistentDataPath, FileName);

    public static void Save(InventorySaveData data)
    {
        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(SavePath, json);

        Debug.Log($"Save: {SavePath}");
    }

    public static InventorySaveData Load()
    {
        if (!File.Exists(SavePath)) return null;

        string json = File.ReadAllText(SavePath);

        return JsonUtility.FromJson<InventorySaveData>(json);
    }
}