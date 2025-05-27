using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SaveAllData", menuName = "ScriptableObjects/SaveAllData", order = 1)]
public class SaveAllData : ScriptableObject
{
    public int money;
    public List<Texture> textures;
}

