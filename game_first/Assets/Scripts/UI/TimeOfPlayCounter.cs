using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TimeOfPlayCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textOfScore;


    private void Update()
    {
        if (Time.timeScale == 0 || GameModel.PlayerHitPoint is null) return;
        textOfScore.text = (Time.timeSinceLevelLoad * 100).ToString("F0");
        Statistic.sessionScore = int.Parse(textOfScore.text);
    }
}
