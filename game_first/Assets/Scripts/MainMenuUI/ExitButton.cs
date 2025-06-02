using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public SaveAllData gameData;

    public void ExitGame()
    {
        Application.Quit();
    }
}
