using UnityEngine;
using TMPro;

public class BuyButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyTextMesh;
    
    public void Buy()
    {
        if (TextureManager.selectedTextureCost <= Statistic.PlayersMoney &&
            !TextureManager.PlayersTextures.Contains(TextureManager.selectedTexture))
        {
            Statistic.PlayersMoney -= TextureManager.selectedTextureCost;
            moneyTextMesh.text = Statistic.PlayersMoney.ToString();
            TextureManager.PlayersTextures.Add(TextureManager.selectedTexture);
        }
    }
}
