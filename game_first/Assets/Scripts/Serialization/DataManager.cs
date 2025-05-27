using UnityEngine;

public class DataManager : MonoBehaviour
{
    public SaveAllData gameData;

    void Start()
    {
        GameModel.playersMoney = gameData.money;
        foreach (var texture in gameData.textures)
            GameModel.playersTextures.Add(texture);
    }
}

