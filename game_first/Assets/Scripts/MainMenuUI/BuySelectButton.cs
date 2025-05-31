using UnityEngine;
using TMPro;

public class BuySelectButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI costTextMesh;
    [SerializeField] private TextMeshProUGUI textMesh;
    
    public void BuySelectSkin()
    {
        if (!TextureManager.PlayersTextures.Contains(TextureManager.selectedTexture))
        {
            if (Statistic.PlayersMoney >= TextureManager.selectedTextureCost)
            {
                Statistic.PlayersMoney -= TextureManager.selectedTextureCost;
                costTextMesh.text = TextureManager.PlayersTextures.ToString();
                textMesh.text = "Not selected";
                TextureManager.PlayersTextures.Add(TextureManager.selectedTexture);
            }
            else
                textMesh.text = "Not enough money";
        }
        else
        {
            textMesh.text = "Selected";
            TextureManager.CurrentTexture = TextureManager.selectedTexture;
        }
    }
}
