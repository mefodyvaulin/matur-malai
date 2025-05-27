using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public SaveAllData gameData;

    public void ExitGame()
    {
        gameData.money = GameModel.playersMoney;
        foreach (var texture in GameModel.playersTextures)
            if (!gameData.textures.Contains(texture))
                gameData.textures.Add(texture);
    }
}
