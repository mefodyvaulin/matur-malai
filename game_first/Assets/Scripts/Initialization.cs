using TMPro;
using UnityEngine;

public class Initialization : MonoBehaviour
{
    [SerializeField] GameObject xWing;

    private void Awake()
    {
        GameModel.SetPlayerTransform(xWing.transform);
    }
}
