using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneTo : MonoBehaviour
{
    [SerializeField] private string sceneName;
    
    public void StartGameScene(bool education)
    {
        AudioListener.pause = false;
        GameModel.ResetModel();
        Cursor.visible = sceneName != "MefodiyCube";
        Cursor.lockState = sceneName != "MefodiyCube"? CursorLockMode.None: CursorLockMode.Locked;
        if (sceneName == "MainMenuScene") Statistic.SaveStat();
        SceneManager.LoadSceneAsync(sceneName);
        GameModel.isEducation = education;
        Time.timeScale = 1;
    }
}