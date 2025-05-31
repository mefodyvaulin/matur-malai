using UnityEngine;

public class SelectSkinButton : MonoBehaviour
{
    public void SelectSkin()
    {
        if (TextureManager.PlayersTextures.Contains(TextureManager.selectedTexture))
        {
            TextureManager.CurrentTexture = TextureManager.selectedTexture;
        }
    }
}
