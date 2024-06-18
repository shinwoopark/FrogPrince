using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    public static GameInstance instance;

    public bool bPlay;

    //Player
    public int TrasformLevel;
    public float CurrentHp;
    public float MaxHp;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);

        bPlay = true;
    }

    public void GameStart()
    {
        bPlay = true;
        TrasformLevel = 1;
        CurrentHp = MaxHp;
    }
}
