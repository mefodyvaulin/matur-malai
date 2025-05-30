using UnityEngine;

public class SelectSkinButton : MonoBehaviour
{
    public void SelectSkin()
    {
        if (GameModel.PlayersTextures.Contains(GameModel.selectedTexture))
        {
            GameModel.CurrentTexture = GameModel.selectedTexture;
        }
    }
}
