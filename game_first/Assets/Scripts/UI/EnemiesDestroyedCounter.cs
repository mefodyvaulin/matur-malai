using TMPro;
using UnityEngine;

public class EnemysDestroyedCounter : MonoBehaviour
{
    private int score;
    [SerializeField] private TextMeshProUGUI textOfTime;
    void Update()
    {
        if (score == GameModel.Score) return;
        score = GameModel.Score;
        textOfTime.text = $"Enemies destroyed: {score}";
    }
}
