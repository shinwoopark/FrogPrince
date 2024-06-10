using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Update()
    {
        UpdateHp();
    }

    private void UpdateHp()
    {
        if (GameInstance.instance.CurrentHp <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {

    }
}
