using System.Collections.Generic;
using UnityEngine;

public static class TextureManager
{
    public static SaveAllData saveAllData;

    public static Texture selectedTexture;
    public static int selectedTextureCost;

    public static Texture CurrentTexture
    {
        get => saveAllData.currentTexture;
        set => saveAllData.currentTexture = value;
    }
    public static List<Texture> PlayersTextures
    {
        get => saveAllData.playersTextures;
        set => saveAllData.playersTextures = value;
    }





}
