using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneTo : MonoBehaviour
{
    [SerializeField] private string sceneName;
    
    public void StartGameScene()
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
