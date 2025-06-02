using UnityEngine;

public class DataManager : MonoBehaviour
{
    public SaveAllData gameData;
    private static bool isGameInitiallyStarted = true;

    private void Start()
    {
        if (isGameInitiallyStarted)
        {
            isGameInitiallyStarted = false;
            SaveLoadManager.LoadData(gameData);
        }
        TextureManager.saveAllData = gameData;
        Statistic.saveAllData = gameData;
    }
    
    private void OnApplicationQuit()
    {
        SaveLoadManager.SaveData(gameData);
    }
}

