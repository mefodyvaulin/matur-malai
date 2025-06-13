using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SaveAllData", menuName = "ScriptableObjects/SaveAllData", order = 1)]
public class SaveAllData : ScriptableObject
{
    public int playersMoney;
    public int bestScore;
    public List<int> lastScores;
    public Texture currentTexture;
    public List<Texture> playersTextures;
    public bool isFirstGame = true;
    public bool isJsonCreated = false;
}

