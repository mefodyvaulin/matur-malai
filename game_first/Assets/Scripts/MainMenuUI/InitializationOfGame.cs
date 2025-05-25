using UnityEngine;

public class InitializationOfGame : MonoBehaviour
{
    [SerializeField] Texture defaultTexture;
    [SerializeField] Material material;
    private void Awake()
    {
        material.SetTexture("_MainTex", defaultTexture);
        GameModel.currentTexture = defaultTexture;
        GameModel.selectedTexture = defaultTexture;
        GameModel.playersTextures.Add(defaultTexture);
    }
}
