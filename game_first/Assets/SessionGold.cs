using TMPro;
using UnityEngine;

public class SessionGold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textOfGold;

    // Update is called once per frame
    void Update()
    {
        textOfGold.text = Statistic.playerSessionMoney.ToString();
    }
}
