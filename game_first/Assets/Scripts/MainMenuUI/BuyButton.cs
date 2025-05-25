using UnityEngine;
using TMPro;

public class BuyButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyTextMesh;
    
    public void Buy()
    {
        if (GameModel.selectedTextureCost <= GameModel.playersMoney &&
            !GameModel.playersTextures.Contains(GameModel.selectedTexture))
        {
            GameModel.playersMoney -= GameModel.selectedTextureCost;
            moneyTextMesh.text = GameModel.playersMoney.ToString();
            GameModel.playersTextures.Add(GameModel.selectedTexture);
        }
    }
}
