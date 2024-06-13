using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss1Pattern1 : MonoBehaviour
{
    public Bullet WindBullet;

    public Transform Player_tr;

    public Boss1StateSystem Boss1StateSystem;

    public Transform WayPoint, FinishPoint;

    public GameObject WindBullet_gb;

    private int _wayPointNumber;

    private Vector3 _wayPointDir;

    private bool _bFindWayPoint;

    private int _hitWayPoint;

    public float MoveSpeed;

    private int _dir;

    private bool _bFinishMove;

    private void Update()
    {
        if (Boss1StateSystem.CurrentState == Boss1State.Pattern1)
        {
            FindWayPoint();
        }         
    }

    private void FixedUpdate()
    {
        if (Boss1StateSystem.CurrentState == Boss1State.Pattern1)
        {
            UpdateFollowWayPoint();
        }         
    }

    public void StartPattern(int dir)
    {
        _dir = dir;

        if (dir == 1)
        {
            _wayPointNumber = 0;
        }
        else if(dir == -1)
        {
            _wayPointNumber = 4;
        }

        SpawnWayPoint();
        _hitWayPoint = 0;
        _bFindWayPoint = true;
        _bFinishMove = false;
    }

    private void FindWayPoint()
    {
        if (_bFindWayPoint && _hitWayPoint < 5) 
        {
            _wayPointDir = WayPoint.GetChild(_wayPointNumber).position - transform.position;          
        }
        else
        {
            _wayPointDir = FinishPoint.position - transform.position;
        }

        _wayPointDir.Normalize();
    }

    private void UpdateFollowWayPoint()
    {
        if (!_bFinishMove)
        {
            transform.Translate(_wayPointDir * MoveSpeed * Time.deltaTime);
        }       
    }

    IEnumerator WindAttack()
    {
        Debug.Log("!");
        yield return new WaitForSeconds(1);
        Vector3 dir = transform.position - Player_tr.position;
        Instantiate(WindBullet_gb, transform.position, Quaternion.identity);

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

        if (_dir == 1)
        {
            FinishPoint.position = new Vector3(11, 3, 0);
        }
        else if (_dir == -1)
        {
            FinishPoint.position = new Vector3(-11, 3, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Boss1StateSystem.CurrentState == Boss1State.Pattern1)
        {
            if (collision.gameObject.tag == "WayPoint")
            {
                if (collision.gameObject == WayPoint.GetChild(_wayPointNumber).gameObject)
                {
                    _hitWayPoint++;

                    if (_dir == 1)
                        _wayPointNumber++;
                    else if (_dir == -1)
                        _wayPointNumber--;
                }
            }

            if (collision.gameObject.tag == "FinishPoint")
            {
                _bFinishMove = true;
                StartCoroutine(WindAttack());
            }
        }        
    }
}
