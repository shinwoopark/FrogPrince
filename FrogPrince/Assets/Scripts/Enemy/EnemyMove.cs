using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private EnemyStateSystem _enemyState; 

    private SpriteRenderer _spriteRenderer;

    public float MoveSpeed;

    public int Dir;
    public bool RandomTimeDirSet;
    private float _randomTime;
    public float MiniTime, MaxTime;

    private void Awake()
    {
        _enemyState = GetComponent<EnemyStateSystem>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        RandomTimeDirSeting();
    }

    private void Update()
    {
        if (_enemyState.CurrentState != EnemyState.Death)
        {
            UpdateDir();
        }          
    }

    private void FixedUpdate()
    {
        if (_enemyState.CurrentState != EnemyState.Death)
        {
            UpdateMove();
        }        
    }  

    private void UpdateMove()
    {
        if(_enemyState.CurrentState == EnemyState.Forward)
        {
            transform.position += new Vector3(MoveSpeed * Dir * Time.deltaTime, 0, 0);
        }
    }

    private void UpdateDir()
    {
        if (Dir == 1)
        {
            _spriteRenderer.flipY = false;
        }
        else if (Dir == -1)
        {
            _spriteRenderer.flipY = true;
        }

        //if (_enemyState.bWall)
        //{
        //    if (Dir == 1)
        //    {
        //        Dir = -1;
        //    }
        //    else if (Dir == -1)
        //    {
        //        Dir = 1;
        //    }
        //}
    }

    private void RandomTimeDirSeting()
    {
        if (RandomTimeDirSet)
        {
            Dir *= -1;
            _randomTime = Random.Range(MiniTime, MaxTime);           
            Invoke("RandomTimeDirSeting", _randomTime);
        }
    }
}
