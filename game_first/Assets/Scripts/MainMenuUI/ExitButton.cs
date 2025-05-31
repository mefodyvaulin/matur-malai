using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public SaveAllData gameData;

    public void ExitGame()
    {
        foreach (var texture in TextureManager.PlayersTextures)
            if (!gameData.playersTextures.Contains(texture))
                gameData.playersTextures.Add(texture);
    }
}
