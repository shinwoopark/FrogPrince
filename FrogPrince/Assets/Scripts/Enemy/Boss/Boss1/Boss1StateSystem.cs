using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Boss1State
{
    PatternCool,
    Pattern1,
    Pattern2
}

public class Boss1StateSystem : MonoBehaviour
{
    public Boss1State CurrentState = Boss1State.PatternCool;

    private Boss1Pattern1 _boss1Pattern1;

    private void Awake()
    {
        _boss1Pattern1 = GetComponent<Boss1Pattern1>();
        CallPattern1();
    }

    private void CallPattern1()
    {
        int dir = 0;

        if(transform.position.x < 0)
        {
            dir = 1;
        }
        else
        {
            dir = -1;
        }

        _boss1Pattern1.StartPattern(dir);
    }
}
