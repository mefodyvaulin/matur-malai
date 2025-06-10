using TMPro;
using UnityEngine;

public class TimeOfPlayCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textOfScore;

    private void Update()
    {
        if (Time.timeScale == 0 || GameModel.PlayerHitPoint is null) return;
        var boost = 1;
        if (GameModel.SpeedBuff != null) boost = GameModel.SpeedBuff.boost * 10;
        Statistic.sessionScore += Time.deltaTime * 10 * boost;
        textOfScore.text = ((int)Statistic.sessionScore).ToString("F0");
    }
}
