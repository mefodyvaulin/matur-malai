using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    private bool paused;

    private void Awake()
    {
        InputManager.PauseON.performed += PauseOnAction;
        InputManager.PauseOFF.performed += PauseOnAction;
    }

    void OnEnable()
    {
        InputManager.PauseON.Enable();
        InputManager.PauseON.performed += PauseOnAction;

        InputManager.PauseOFF.Enable();
        InputManager.PauseOFF.performed += PauseOnAction;

    }

    void OnDisable()
    {
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
