using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss1Pattern1 : MonoBehaviour
{
    private Boss1StateSystem _boss1StateSystem;

    public BulletSystem WindBullet;

    public Transform Player_tr;  

    public Transform StartPoint, WayPoint, FinishPoint, BulletPos;

    public GameObject WindBullet_gb;

    private SpriteRenderer _spriteRenderer;

    private int _wayPointNumber;

    private Vector3 _wayPointDir;

    private bool _bFindWayPoint;

    private int _hitWayPoint;

    public float MoveSpeed;

    private int _dir;

    private bool _bReady;

    private bool _bFinishMove;

    private void Awake()
    {
        _boss1StateSystem = GetComponent<Boss1StateSystem>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_boss1StateSystem.CurrentState == Boss1State.Pattern1)
        {
            FindWayPoint();
        }      
    }

    private void FixedUpdate()
    {
        if (_boss1StateSystem.CurrentState == Boss1State.Pattern1)
        {
            UpdateMove();
        }
    }

    public void StartPattern()
    {
        int dir = Random.Range(0, 2);

        if (dir == 0)
        {
            _wayPointNumber = 0;
            _dir = 1;
            _spriteRenderer.flipX = true;
        }
        else if(dir == 1)
        {
            _wayPointNumber = 4;
            _dir = -1;
            _spriteRenderer.flipX = false;
        }

        SpawnWayPoint();
        _hitWayPoint = 0;
        _bReady = false;
        _bFindWayPoint = true;
        _bFinishMove = false;
    }

    private void FindWayPoint()
    {
        if (!_bReady)
        {
            _wayPointDir = StartPoint.position - transform.position;
        }
        else if (_bFindWayPoint && _hitWayPoint < 5) 
        {
            _wayPointDir = WayPoint.GetChild(_wayPointNumber).position - transform.position;
            
            if(_dir < 1)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }
        }
        else
        {
            _wayPointDir = FinishPoint.position - transform.position;
        }

        _wayPointDir.Normalize();
    }

    private void UpdateMove()
    {
        if (!_bFinishMove)
        {
            transform.position += _wayPointDir * MoveSpeed * Time.deltaTime;
        }
    }

    IEnumerator WindAttack()
    {
        if (Player_tr.position.x > transform.position.x)
        {
            _spriteRenderer.flipX = false;
        }
        else if (Player_tr.position.x < transform.position.x)
        {
            _spriteRenderer.flipX = true;
        }

        yield return new WaitForSeconds(1);

        Vector3 dir = Player_tr.position - transform.position;
        dir.Normalize();

        FireWind(dir);
        FireWind(dir -= new Vector3(0.5f, 0, 0));
        FireWind(dir -= new Vector3(-1.5f, 0, 0));
    }

    private void FireWind(Vector3 dir)
    {
        WindBullet = WindBullet_gb.GetComponent<BulletSystem>();
        WindBullet.Dir = dir;
        Instantiate(WindBullet_gb, BulletPos.position, Quaternion.identity);
    }

    private void SpawnWayPoint()
    {
        for(int i = 0; i < 5; i++)
        {
            int pattern = Random.Range(0, 3);

            float x = -10;

            for (int j = 0; j < i; j++)
            {
                x -= -5;
            }

            switch(pattern)
            {
                case 0:
                    WayPoint.GetChild(i).position = new Vector3(x, -3, 0);
                    break;
                case 1:
                    WayPoint.GetChild(i).position = new Vector3(x, -1.5f, 0);
                    break;
                case 2:
                    WayPoint.GetChild(i).position = new Vector3(x, 0, 0);
                    break;
            }
        }

        if (_dir == -1)
        {
            StartPoint.position = new Vector3(20, 0, 0);
            FinishPoint.position = new Vector3(-11, 3, 0);
        }
        else if (_dir == 1)
        {
            StartPoint.position = new Vector3(-20, 0, 0);
            FinishPoint.position = new Vector3(11, 3, 0);
        }
    }

    private void EndPattern()
    {
        _boss1StateSystem.CallPattern();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_boss1StateSystem.CurrentState == Boss1State.Pattern1)
        {
            if (collision.gameObject.tag == "StartPoint")
            {
                _bReady = true;
            }

            if (collision.gameObject.tag == "WayPoint")
            {
                if (collision.gameObject == WayPoint.GetChild(_wayPointNumber).gameObject)
                {
                    _hitWayPoint++;

                    if (_dir == 1
                        && _hitWayPoint < 5)
                    {
                        _wayPointNumber++;
                    }
                    else if (_dir == -1
                        && _hitWayPoint < 5)
                    {
                        _wayPointNumber--;
                    }
                }
            }

            if (collision.gameObject.tag == "FinishPoint")
            {
                _bFinishMove = true;
                StartCoroutine(WindAttack());
                Invoke("EndPattern", 2);
            }
        }          
    }
}
