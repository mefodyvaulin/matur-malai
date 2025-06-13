using System.IO;
using UnityEngine;

public static class SaveLoadManager
{
    private static string filePath => Path.Combine(Application.dataPath, "gameData.json");

    public static void SaveData(SaveAllData gameData)
    {
        string json = JsonUtility.ToJson(gameData);
        File.WriteAllText(filePath, json);
        //Debug.Log("Game data saved to " + filePath);
    }

    public static void LoadData(SaveAllData gameData)
    {
        string json = File.ReadAllText(filePath);
        JsonUtility.FromJsonOverwrite(json, gameData);
        //Debug.Log("Game data loaded from " + filePath);
    }
}

