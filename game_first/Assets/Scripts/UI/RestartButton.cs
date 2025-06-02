using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    private string sceneName = "MefodiyCube";
    
    public void RestartCurrentScene()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Statistic.SaveStat();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

