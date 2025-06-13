using UnityEngine;

public class DataManager : MonoBehaviour
{
    public SaveAllData gameData;
    private static bool isGameInitiallyStarted = true;
    [SerializeField] Material XwingTexture;

    private void Start()
    {
        if (isGameInitiallyStarted)
        {
            isGameInitiallyStarted = false;
            SaveLoadManager.LoadData(gameData);
        }
        TextureManager.saveAllData = gameData;
        Statistic.saveAllData = gameData;
        XwingTexture.SetTexture("_MainTex", TextureManager.CurrentTexture);
    }
    
    private void OnApplicationQuit()
    {
        SaveLoadManager.SaveData(gameData);
    }
}

