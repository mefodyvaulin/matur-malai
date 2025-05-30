using UnityEngine;

public class DataManager : MonoBehaviour
{
    public SaveAllData gameData;

    private void Start()
    {
        GameModel.saveAllData = gameData;
    }
}

