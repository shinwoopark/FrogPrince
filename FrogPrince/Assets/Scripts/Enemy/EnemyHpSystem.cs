using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class EnemyHpSystem : MonoBehaviour
{
    private EnemyStateSystem _enemyState;

    private SpriteRenderer _spriteRenderer;

    public int MaxHp;
    public int CurrentHp;

    private void Awake()
    {
        _enemyState = GetComponent<EnemyStateSystem>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
        CurrentHp -= 1;
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
