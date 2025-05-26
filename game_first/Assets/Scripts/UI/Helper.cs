using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Helper : MonoBehaviour
{
    [SerializeField] public TextMeshPro text;
    [SerializeField] public Animator animator;
    private Dictionary<string, InputAction> tasks;
    Vector3 direction;
    public AudioSource audio;

    private void Start()
    {
        tasks = new Dictionary<string, InputAction>
        {
            { "Стрельба на левую кнопку мыши.", InputManager.LeftClick },
            { "Передвигай мышь, чтобы поворачивать.", InputManager.MouseMove },
            { "Суперудар на пробел.", InputManager.Ulta },
            { "Нажми ESC для приостановки игры.", InputManager.PauseON }
        };
        StartCoroutine(StartEducation());
    }

    private IEnumerator StartEducation()
    {
        foreach (var task in tasks)
        {
            yield return StartCoroutine(PrintText(task.Key));
            yield return StartCoroutine(CheckAns(task.Value));
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator CheckAns(InputAction task)
    {
        while (true)
        {
            if (task.IsPressed())
            {
                SwitchAnim("wait", "yes");
                break;
            }
            yield return new WaitForSeconds(1f);
        }
        SwitchAnim("yes", "wait");
    }

    private void Update()
    {
        var desiredPosition = GameModel.PlayerPosition + Vector3.forward * 10f;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.1f);
    }

    private IEnumerator PrintText(string str)
    {
        var text0 = "";
        SwitchAnim("wait", "educate");
        for (var i = 0; i < str.Length; i++)
        {
            text0 += str[i];
            text.text = text0;
            yield return new WaitForSeconds(0.1f);
        }
        SwitchAnim("educate", "wait");
    }

    private void SwitchAnim(string off, string on)
    {
        animator.SetBool(off, false);
        animator.SetBool(on, true);
    }
}