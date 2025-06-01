using System.Linq;
using UnityEngine;
using TMPro;

public class ScoringButton : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsFromOtherUI;
    [SerializeField] private GameObject[] objectsFromScoringUI;
    [SerializeField] private TextMeshProUGUI[] scoreTexts; // нужен правильный порядок
    [SerializeField] private TextMeshProUGUI scoreText;
    private bool atScoringRoom = false;

    public void OnScoringClick()
    {
        if (!atScoringRoom)
        {
            atScoringRoom = true;
            foreach (var obj in objectsFromOtherUI)
                obj.SetActive(false);
            foreach (var obj in objectsFromScoringUI)
                obj.SetActive(true);
            scoreText.text = "To menu";
            if (Statistic.LastGamesScore is not null)
            {
                var sortedStatistic = Statistic.LastGamesScore.OrderByDescending(n => n).ToList();
                for (int i = 0; i < Statistic.LastGamesScore.Count; i++)
                    scoreTexts[i].text = sortedStatistic[i].ToString();
            }
        }
        else
        {
            atScoringRoom = false;
            foreach (var obj in objectsFromOtherUI)
                obj.SetActive(true);
            foreach (var obj in objectsFromScoringUI)
                obj.SetActive(false);
            scoreText.text = "Records";
        }
    }
}
