using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Boss1State
{
    Idle,
    Pattern1,
    Pattern2,
    Death
}

public class Boss1StateSystem : MonoBehaviour
{
    public Boss1State CurrentState = Boss1State.Idle;

    public GameManager GameManager;

    private Boss1Pattern1 _boss1Pattern1;
    private Boss1Pattern2 _boss1Pattern2;

    private int _patternCount = 0;

    private SpriteRenderer _spriteRenderer;

    public int MaxHp;
    public int CurrentHp;

    private void Awake()
    {
        _boss1Pattern1 = GetComponent<Boss1Pattern1>();
        _boss1Pattern2 = GetComponent<Boss1Pattern2>();
        _spriteRenderer = GetComponent<SpriteRenderer>();     
    }

    private void Start()
    {
        CallPattern();
        CurrentHp = MaxHp;
    }

    private void Update()
    {
        UpdateHp();
        UpdateDeath();
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

    private void UpdateHp()
    {
        if (CurrentHp <= 0)
        {
            CurrentState = Boss1State.Death;
        }

        if (CurrentHp > MaxHp)
        {
            CurrentHp = MaxHp;
        }
    }

    public void HpDown()
    {
        if (CurrentState != Boss1State.Death)
        {
            CurrentHp -= 1;
            StartCoroutine(TurnRed());
        }
    }

    IEnumerator TurnRed()
    {
        _spriteRenderer.color = Color.red * 0.8f;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
    }

    public void UpdateDeath()
    {
        if (CurrentState == Boss1State.Death)
        {
            _spriteRenderer.color -= new Color(0, 0, 0, 0.5f * Time.deltaTime);

            if (_spriteRenderer.color.a <= 0)
            {
                gameObject.layer = 1;
                GameManager.GameClear();
                Destroy(gameObject);
            }
        }
    }
}
