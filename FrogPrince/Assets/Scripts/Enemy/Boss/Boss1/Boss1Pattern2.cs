using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Pattern2 : MonoBehaviour
{
    public Transform[] Flowers_tr;

    private int _chooseFlower;

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Flower")
        {

        }
    }
}
