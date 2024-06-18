using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1WindBomb : BulletSystem
{
    public AudioSource AudioSource;

    public float DeathTimer;

    private void Start()
    {
        AudioSource.Play();
    }

    private void Update()
    {
        DeathTimer -= Time.deltaTime;

        if (DeathTimer <= 0)
        {
            Destroy(gameObject);
        }     
    }
}
