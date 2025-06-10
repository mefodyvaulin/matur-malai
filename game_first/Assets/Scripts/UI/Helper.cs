using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class Helper : MonoBehaviour
{
    [SerializeField] public TextMeshPro text;
    [SerializeField] public Animator animator;
    private Dictionary<string, InputAction> tasks;
    Vector3 direction;
    private bool end;
    public static bool helperAlive = false; //включается в OnEnable
    public AudioSource audio;

    private void Start()
    {
        tasks = new Dictionary<string, InputAction>
        {
            { "Передвигай мышь, чтобы поворачивать.", InputManager.MouseMove },
            { "Стрельба на левую кнопку мыши.", InputManager.LeftClick },
            { "Хп и патроны отображаются справа сверху.", InputManager.MouseMove},
            { "Суперудар на пробел. Он отображается слева сверху.", InputManager.Ulta },
            { "Нажми ESC для приостановки игры.", InputManager.PauseOFF },
            { "Во время игры встречаются баффы обязательно подбирай их.", InputManager.MouseMove},
            { "За звездочки можно покупать крутые скины в магазине.", InputManager.MouseMove}
        };
        StartCoroutine(Education());
    }

    private void OnEnable()
    {
        helperAlive = true;
    }

    private IEnumerator Education()
    {
        foreach (var task in tasks)
        {
            if (Time.timeScale == 0)
            {
                yield return null;
                continue;
            }
            yield return StartCoroutine(PrintText(task.Key));
            yield return StartCoroutine(CheckAns(task.Value));
            yield return new WaitForSeconds(1f);
        }

        end = true;
    }

    private IEnumerator CheckAns(InputAction task)
    {
        var timeStarted = GameModel.UnscaledTime;
        while (true)
        {
            if (Time.timeScale == 0)
            {
                yield return null;
                continue;
            }
            if (task.IsPressed())
            {
                SwitchAnim("wait", "yes");
                yield return new WaitForSeconds(1f);
                break;
            }

            if (GameModel.UnscaledTime - timeStarted > 4)
                break;
            yield return null;
        }
        SwitchAnim("yes", "wait");
    }

    private void Update()
    {

        var player = GameModel.PlayerPosition;
        var trenchRightUp = GameModel.PlayerMovement.trenchSizeUpRight;
        var trenchDownLeft = GameModel.PlayerMovement.trenchSizeDownLeft;
        if (end)
        {
            SwitchAnim("wait", "buy");
            StartCoroutine(EndEducate(trenchRightUp));
        }
        var desiredPosition = new Vector3(
            (trenchDownLeft.x + trenchRightUp.x)/2,
            trenchRightUp.y - 5,
            player.z) + Vector3.forward * 20;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, 1f);

    }

    private IEnumerator EndEducate(Vector2 trenchRightUp)
    {
        yield return new WaitForSeconds(2f);
        transform.position = Vector3.Lerp(transform.position, transform.position * 10, 1);
        if (transform.position.y > trenchRightUp.y + 50)
        {
            helperAlive = false;
            GameModel.GenerateTrench.locationSegmentIndex = 0;
            GameModel.GenerateTrench.locationIndex = 0;
            GameModel.GenerateTrench.ReloadSegments();
            Destroy(gameObject);
        }
    }

    private IEnumerator PrintText(string str)
    {
        var text0 = "";
        SwitchAnim("wait", "educate");
        for (var i = 0; i < str.Length; i++)
        {
            if (Time.timeScale == 0)
            {
                yield return null;
            }
            text0 += str[i];
            text.text = text0;
            yield return new WaitForSeconds(0.02f);
        }
        SwitchAnim("educate", "wait");
    }

    private void SwitchAnim(string off, string on)
    {
        animator.SetBool(off, false);
        animator.SetBool(on, true);
    }
}