using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneTo : MonoBehaviour
{
    [SerializeField] private string sceneName;
    
    public void StartGameScene()
    {
        AudioListener.pause = false;
        GameModel.ResetModel();
        if (sceneName == "MainMenuScene") Statistic.SaveStat();
        SceneManager.LoadSceneAsync(sceneName);
        Time.timeScale = 1;
    }

    public void StartGameWithEducation()
    {
        AudioListener.pause = false;
        GameModel.ResetModel();
        if (sceneName == "MainMenuScene") Statistic.SaveStat();
        SceneManager.LoadSceneAsync(sceneName);
        GameModel.isEducation = true;
        Time.timeScale = 1;
    }

}