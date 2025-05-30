using UnityEngine;
using TMPro;

public class BuyButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyTextMesh;
    
    public void Buy()
    {
        if (GameModel.selectedTextureCost <= GameModel.PlayersMoney &&
            !GameModel.PlayersTextures.Contains(GameModel.selectedTexture))
        {
            GameModel.PlayersMoney -= GameModel.selectedTextureCost;
            moneyTextMesh.text = GameModel.PlayersMoney.ToString();
            GameModel.PlayersTextures.Add(GameModel.selectedTexture);
        }
    }
}
