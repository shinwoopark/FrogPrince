using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1WindBullet : MonoBehaviour
{
    private BulletSystem _bulletSystem;

    public GameObject WindBomb;

    public float VectorX, VectorY;

    private void Awake()
    {
        _bulletSystem = GetComponent<BulletSystem>();
    }

    private void Start()
    {
        SetMovement();
    }

    private void Update()
    {
        HitSystem();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    private void HitSystem()
    {
        if (_bulletSystem.bHitPlayer || _bulletSystem.bHitPlatform)
        {
            Bomb();
            Destroy(gameObject);
        }
    }

    private void UpdateMovement()
    {
        transform.position += new Vector3(VectorX, VectorY, 0);
    }

    private void SetMovement()
    {
        VectorX *= -1;
        Invoke("SetMovement", 0.1f);
    }

    private void Bomb()
    {
        Instantiate(WindBomb, transform.position, Quaternion.identity);
    }    
}
