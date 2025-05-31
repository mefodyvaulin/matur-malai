using TMPro;
using UnityEngine;

public class MoneyUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
       textMesh.text = Statistic.PlayersMoney.ToString();
    }
}
