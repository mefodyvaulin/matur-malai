using UnityEngine;

public class Initialization : MonoBehaviour
{
    [SerializeField] GameObject xWing;
    [SerializeField] private GameObject helper;

    private void Awake()
    {
        GameModel.SetPlayerTransform(xWing.transform);
        if (GameModel.isEducation) helper.SetActive(true);
    }
}
