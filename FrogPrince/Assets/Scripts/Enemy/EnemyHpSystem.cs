using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpSystem : MonoBehaviour
{
    private EnemyStateSystem _enemyState;

    private SpriteRenderer _spriteRenderer;

    public int MaxHp;
    public int CurrentHp;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyState = GetComponent<EnemyStateSystem>();
    }

    private void Start()
    {
        CurrentHp = MaxHp;
    }

    private void Update()
    {
        UpdateHp();
        UpdateDeath();
    }

    private void UpdateHp()
    {
        if (CurrentHp <= 0)
        {
            _enemyState.CurrentState = EnemyState.Death;
        }

        if (CurrentHp > MaxHp)
        {
            CurrentHp = MaxHp;
        }
    }

    public void HpDown()
    {
        if(_enemyState.CurrentState != EnemyState.Death)
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
        if (_enemyState.CurrentState == EnemyState.Death)
        {
            _spriteRenderer.color -= new Color(0, 0, 0, 1 * Time.deltaTime);

            if (_spriteRenderer.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
