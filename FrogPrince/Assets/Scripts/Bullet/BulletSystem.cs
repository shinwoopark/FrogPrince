using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSystem : MonoBehaviour
{
    public float Damage;

    public float MoveSpeed;

    [HideInInspector]
    public Vector3 Dir;

    [HideInInspector]
    public bool bHitPlayer, bHitPlatform;

    private void FixedUpdate()
    {
        UpdateMove();
    }

    private void UpdateMove()
    {
        transform.position += Dir * MoveSpeed * Time.deltaTime;    
    }

    public void SetDir()
    {
        float rotZ = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
            bHitPlayer = true;

        if (collision.gameObject.layer == 6
            || collision.gameObject.layer == 7
            || collision.gameObject.layer == 8)
            bHitPlatform = true;
    }
}
