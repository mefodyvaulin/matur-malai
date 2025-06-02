using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FirstStartGame : MonoBehaviour
{
    [SerializeField] GameObject firstStartGamePanel;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] private SaveAllData gameData;

    private IEnumerator Start()
    {
        while (gameData == null)
        {
            yield return null;
        }

        if (gameData.isFirstGame)
        {
            firstStartGamePanel.SetActive(true);
            while (!firstStartGamePanel.activeInHierarchy)
            {
                yield return null;
            }

            StartCoroutine(PrintText("Привет! \n Нажми на вопрос, чтобы начать игру"));
            gameData.isFirstGame = false;
        }
    }

    private IEnumerator PrintText(string str)
    {
        var text0 = "";
        for (var i = 0; i < str.Length; i++)
        {
            text0 += str[i];
            text.text = text0;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
