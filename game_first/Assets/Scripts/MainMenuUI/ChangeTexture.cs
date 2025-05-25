using UnityEngine;
using TMPro;

public class ChangeTexture : MonoBehaviour
{
    public Material material; 
    public Texture newTexture;
    [SerializeField] private int costOfTexture;
    [SerializeField] private TextMeshProUGUI costTextMesh;
    
    public void ChangeMaterialTexture()
    {
        if (material != null && newTexture != null)
        {
            if (GameModel.playersTextures.Contains(newTexture))
                costTextMesh.text = "Bought!";
            else
                costTextMesh.text = costOfTexture.ToString();
            material.SetTexture("_MainTex", newTexture);
            GameModel.selectedTexture = newTexture;
            GameModel.selectedTextureCost = costOfTexture;
        }
        else
            Debug.LogWarning("Target Renderer or New Texture is not assigned.");
    }
}

