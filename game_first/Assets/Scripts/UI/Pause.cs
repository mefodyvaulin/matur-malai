using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    private bool paused;
    
    [SerializeField] private PostProcessVolume postProcessVolume; 
    private ChromaticAberration chromaticAberrationEffect;
    private void Awake()
    {
        if (postProcessVolume != null)
            postProcessVolume.profile.TryGetSettings(out chromaticAberrationEffect);
        InputManager.PauseON.performed += PauseOnAction;
        InputManager.PauseOFF.performed += PauseOnAction;
    }

    void OnEnable()
    {
        if (chromaticAberrationEffect != null)
            chromaticAberrationEffect.active = !chromaticAberrationEffect.active;
        InputManager.PauseON.Enable();
        InputManager.PauseON.performed += PauseOnAction;

        InputManager.PauseOFF.Enable();
        InputManager.PauseOFF.performed += PauseOnAction;

    }

    void OnDisable()
    {
        if (chromaticAberrationEffect != null)
            chromaticAberrationEffect.active = !chromaticAberrationEffect.active;
        InputManager.PauseON.performed -= PauseOnAction;
        InputManager.PauseON.Disable();

        InputManager.PauseOFF.performed -= PauseOnAction;
        InputManager.PauseOFF.Disable();
    }


    private void PauseOnAction(InputAction.CallbackContext obj)
    {
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        pauseMenu.SetActive(paused);
        AudioListener.pause = paused;

        if (paused) InputManager.EnableUI();
        else InputManager.EnablePlayer();
    }
}
