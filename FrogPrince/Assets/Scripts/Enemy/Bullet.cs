using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float MoveSpeed;
    [HideInInspector]
    public Vector3 Direction;

    private void FixedUpdate()
    {
        UpdateMove();
    }

    private void UpdateMove()
    {
        transform.Translate(Direction * MoveSpeed * Time.deltaTime);
    }
}
