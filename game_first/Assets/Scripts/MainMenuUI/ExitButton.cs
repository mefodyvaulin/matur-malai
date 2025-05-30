using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public SaveAllData gameData;

    public void ExitGame()
    {
        gameData.playersMoney = GameModel.PlayersMoney;
        foreach (var texture in GameModel.PlayersTextures)
            if (!gameData.playersTextures.Contains(texture))
                gameData.playersTextures.Add(texture);
    }
}
