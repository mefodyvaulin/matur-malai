using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    private string sceneName = "MefodiyCube";
    
    public void RestartCurrentScene()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}

