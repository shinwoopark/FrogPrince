using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject GameOver_ui;
    public GameObject[] Hp_ui;

    private void Update()
    {
        
    }

    public void Hp()
    {
        for (int i = 4; i > 0; i--)
        {
            if (GameInstance.instance.CurrentHp - 1 < i)
                Hp_ui[i].SetActive(false);
        }

        if (GameInstance.instance.CurrentHp <= 0)
        {
            for (int i = 0; i < 5; i++)
            {
                Hp_ui[i].SetActive(false);
            }
            GameOver();
        }
    }

    private void GameOver()
    {
        GameOver_ui.SetActive(true);
        GameInstance.instance.bPlay = false;
    }

    public void GameClear()
    {
        SceneManager.LoadScene("GameClear");
    }

    public void StartGame()
    {
        if (GameInstance.instance != null)
        {
            GameInstance.instance.GameStart();
        }

        SceneManager.LoadScene("Game");
    }

    public void Help()
    {
        SceneManager.LoadScene("Help");
    }

    public void ExitMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
