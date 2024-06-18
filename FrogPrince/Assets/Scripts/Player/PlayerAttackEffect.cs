using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEffect : MonoBehaviour
{
    public float LifeTime;

    private void Update()
    {
        LifeTime -= Time.deltaTime;

        if (LifeTime <= 0)
            Destroy(gameObject);
    }
}
