using UnityEngine;

public class SelectSkinButton : MonoBehaviour
{
    public void SelectSkin()
    {
        if (GameModel.playersTextures.Contains(GameModel.selectedTexture))
        {
            GameModel.currentTexture = GameModel.selectedTexture;
        }
    }
}
