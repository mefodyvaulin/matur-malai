using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TimeOfPlayCounter : MonoBehaviour
{
    [FormerlySerializedAs("text")] [SerializeField] private TextMeshProUGUI textOfScore;

    private void Update()
    {
        if (Time.timeScale == 0) return;
        textOfScore.text = (Time.time * 100).ToString("F0");
        GameModel.sessionScore = int.Parse(textOfScore.text);
    }
}
