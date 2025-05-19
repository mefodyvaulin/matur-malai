using UnityEngine.InputSystem;

public static class InputManager
{
    private static UserInputAction UserInputAction = new();
    public static InputAction MouseMove = UserInputAction.XWing.MouseMovment;
    public static InputAction LeftClick = UserInputAction.XWing.WeaponMovement;
    public static InputAction PauseON = UserInputAction.XWing.Pause;
    public static InputAction PauseOFF = UserInputAction.Pause.Pause;

    public static void EnablePlayer()
    {
        UserInputAction.Pause.Disable();
        UserInputAction.XWing.Enable();
    }

    public static void EnableUI()
    {
        UserInputAction.XWing.Disable();
        UserInputAction.Pause.Enable();
    }

}