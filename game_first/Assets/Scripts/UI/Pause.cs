using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject stat;
    private bool paused;

    private void Awake()
    {
        InputManager.PauseON.Enable();
        InputManager.PauseOFF.Enable();
        InputManager.PauseOFF.performed += PauseOnAction;
        InputManager.PauseON.performed += PauseOnAction;
    }

    private void PauseOnAction(InputAction.CallbackContext obj)
    {
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        pauseMenu.SetActive(paused);
        stat.SetActive(!paused);
        AudioListener.pause = paused;

        if (paused) InputManager.EnableUI();
        else InputManager.EnablePlayer();
    }
}
