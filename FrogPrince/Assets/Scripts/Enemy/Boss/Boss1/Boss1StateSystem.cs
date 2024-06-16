using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Boss1State
{
    Idle,
    Pattern1,
    Pattern2
}

public class Boss1StateSystem : MonoBehaviour
{
    public Boss1State CurrentState = Boss1State.Idle;

    private Boss1Pattern1 _boss1Pattern1;
    private Boss1Pattern2 _boss1Pattern2;

    private int _patternCount = 0;

    private void Awake()
    {
        _boss1Pattern1 = GetComponent<Boss1Pattern1>();
        _boss1Pattern2 = GetComponent<Boss1Pattern2>();
        CallPattern();
    }

    public void CallPattern()
    {
        if (_patternCount < 3)
        {
            CurrentState = Boss1State.Pattern2;
            _boss1Pattern2.StartPattern();
            _patternCount++;
        }
        else
        {
            _boss1Pattern1.StartPattern();
            CurrentState = Boss1State.Pattern1;
            _patternCount = 0;
        }
    }
}
