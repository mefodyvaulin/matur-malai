using UnityEngine;

public class DataManager : MonoBehaviour
{
    public SaveAllData gameData;

    private void Start()
    {
        TextureManager.saveAllData = gameData;
        Statistic.saveAllData = gameData;
    }
}

