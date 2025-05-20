using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject stat;
    private bool paused;
    private float timeScale;

    [SerializeField] private PostProcessVolume postProcessVolume;
    private ChromaticAberration chromaticAberrationEffect;
    private void Awake()
    {
        if (postProcessVolume != null)
            postProcessVolume.profile.TryGetSettings(out chromaticAberrationEffect);
        InputManager.PauseON.Enable();
        InputManager.PauseOFF.Enable();
        InputManager.PauseOFF.performed += PauseOnAction;
        InputManager.PauseON.performed += PauseOnAction;
    }


    private void PauseOnAction(InputAction.CallbackContext obj)
    {
        paused = !paused;
        if (Time.timeScale != 0) timeScale = Time.timeScale;
        Time.timeScale = paused ? 0 : timeScale;
        if(pauseMenu != null) pauseMenu.SetActive(paused);
        if(stat != null) stat.SetActive(!paused);
        if (chromaticAberrationEffect != null)
            chromaticAberrationEffect.active = !chromaticAberrationEffect.active;

        AudioListener.pause = paused;

        if (paused) InputManager.EnableUI();
        else InputManager.EnablePlayer();
    }
}
