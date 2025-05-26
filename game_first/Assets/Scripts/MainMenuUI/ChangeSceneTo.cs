using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneTo : MonoBehaviour
{
    [SerializeField] private string sceneName;
    
    public void StartGameScene()
    {
        AudioListener.pause = false;
        GameModel.ResetModel();
        SceneManager.LoadSceneAsync(sceneName);
        Time.timeScale = 1;
    }

}
