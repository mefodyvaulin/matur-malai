using TMPro;
using UnityEngine;

public class EnemysDestroyedCounter : MonoBehaviour
{
    private int score;
    [SerializeField] private TextMeshProUGUI textOfTime;
    void Update()
    {
        if (score != GameModel.Score)
        {
            score = GameModel.Score;
            textOfTime.text = $"Enemys destroyed: {score}";
        }
    }
}
