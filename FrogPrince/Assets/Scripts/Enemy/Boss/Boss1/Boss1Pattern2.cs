using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Pattern2 : MonoBehaviour
{
    private Boss1StateSystem _boss1StateSystem;
    public FlowerSystem FlowerSystem;

    public FlowerSystem[] FlowerSystems;

    public BulletSystem Boss1White;

    public Transform Flowers_tr, WayPoint;

    public GameObject[] Bullets;

    public float MoveSpeed;

    private int _chooseFlower;

    private int _chooseColor;

    private bool _bMove;

    private bool _bHoneyIntake;

    private bool _bFireHoney;

    private void Awake()
    {
        _boss1StateSystem = GetComponent<Boss1StateSystem>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (_boss1StateSystem.CurrentState == Boss1State.Pattern2)
        {
            UpdateMove();
        }
    }

    public void StartPattern()
    {
        _bMove = true;
        _bHoneyIntake = false;
        _bFireHoney = false;
        ChooseFlower();
        GrowUpFlowers();
    }

    private void UpdateMove()
    {
        if (_bMove)
        {
            if (!_bHoneyIntake)
            {
                Vector3 dir = Flowers_tr.GetChild(_chooseFlower).position - transform.position;
                dir.Normalize();
                transform.Translate(dir * MoveSpeed * Time.deltaTime);
            }
            else if (!_bFireHoney)
            {
                Vector3 dir = WayPoint.GetChild(_chooseFlower).position - transform.position;
                dir.Normalize();
                transform.Translate(dir * MoveSpeed * Time.deltaTime);
            }
        }       
    }

    private void ChooseFlower()
    {
        switch (_chooseFlower)
        {
            case 0:
                _chooseFlower = Random.Range(1, 3);
                break;
            case 2:
                _chooseFlower = Random.Range(0, 2);
                break;
            case 1:
                int random = Random.Range(0, 2);

                if (random == 0)
                {
                    _chooseFlower = 0;
                }
                else if (random == 1)
                {
                    _chooseFlower = 2;
                }
                break;
        }
    }

    IEnumerator HoneyIntake()
    {
        _bMove = false;

        yield return new WaitForSeconds(1);

        _chooseColor = FlowerSystem.Color;     

        _bMove = true;
        _bHoneyIntake = true;

        WitherFlowers();
    }

    IEnumerator Attack()
    {
        _bMove = false;

        yield return new WaitForSeconds(1);

        switch (_chooseColor)
        {
            case 0:
                //for(int i =0; i < 3; i++)
                //{
                    //Instantiate(Bullets[FlowerSystem.Color], transform.position, Quaternion.identity);
                    //yield return new WaitForSeconds(0.5f);
                //}

                for (int i = 0; i < 3; i++)
                {
                    Instantiate(Bullets[_chooseColor], transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(0.5f);
                }
                break;
            case 1:
                for (int i = 0; i < 20; i++)
                {
                    Instantiate(Bullets[_chooseColor], transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(0.1f);
                }

                yield return new WaitForSeconds(0.5f);
                break;
            case 2:
                int numBullets = 36;
                float angleStep = 360.0f / numBullets;
                float radius = 2.0f;

                for (int i = 0; i < numBullets; i++)
                {
                    float angle = i * angleStep;
                    float radian = angle * Mathf.Deg2Rad;
                    float x = radius * Mathf.Cos(radian);
                    float y = radius * Mathf.Sin(radian);

                    Vector3 dir = new Vector3(x, y, 0).normalized;

                    Boss1White = Bullets[_chooseColor].GetComponent<BulletSystem>();
                    
                    Instantiate(Bullets[_chooseColor], transform.position, Quaternion.identity);
                    Boss1White.Dir = dir;                  
                }

                yield return new WaitForSeconds(0.5f);
                break;
        }

        GrowUpFlowers();
        EndPattern();      
    }

    private void EndPattern()
    {
        _boss1StateSystem.CallPattern();
    }

    private void GrowUpFlowers()
    {
        for (int i = 0; i < FlowerSystems.Length; i++)
        {
            FlowerSystems[i].StartCoroutine(FlowerSystems[i].GrowUpFlower());
        }
    }

    private void WitherFlowers()
    {
        for(int i=0; i < FlowerSystems.Length; i++)
        {
            FlowerSystems[i].WitherFlower();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_boss1StateSystem.CurrentState == Boss1State.Pattern2)
        {
            if (collision.gameObject.tag == "Flower")
            {
                FlowerSystem = collision.GetComponent<FlowerSystem>();               
                StartCoroutine(HoneyIntake());
            }

            if (_bHoneyIntake)
            {
                if (collision.gameObject == WayPoint.GetChild(_chooseFlower).gameObject)
                {
                    StartCoroutine(Attack());
                }
            }           
        }
    }
}
