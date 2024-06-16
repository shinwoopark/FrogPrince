using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Forward,
    Attack,
    Death
}

public class EnemyStateSystem : MonoBehaviour
{
    public EnemyState CurrentState = EnemyState.Idle;

    private SpriteRenderer _spriteRenderer;

    public float GroundRayLenth, CeilingRayLenth, WallRayLenth;
    public LayerMask GroundCheck, CeilingCheck, WallCheck;

    public float Damage;
    public float NuckBackPower;

    [HideInInspector]
    public bool bGround;
    [HideInInspector]
    public bool bWall;
    [HideInInspector]
    public bool bCeiling;

    private float _gravityScale;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (CurrentState != EnemyState.Death)
        {
            UpdateGravitySet();
            UpdateRayCast();
        }  
    }

    private void FixedUpdate()
    {
        if (CurrentState != EnemyState.Death)
        {
            UpdateGravity();
        }           
    }

    private void UpdateGravitySet()
    {
        if (!bGround)
        {
            if (CurrentState == EnemyState.Idle
                || CurrentState == EnemyState.Attack)
            {
                _gravityScale = 7.5f;
            }
        }
        else
        {
            _gravityScale = 0;
        }
    }

    private void UpdateGravity()
    {
        transform.position += new Vector3(0, -_gravityScale * Time.deltaTime, 0);
    }

    private void UpdateRayCast()
    {
        float dir = 1;

        if (_spriteRenderer.flipY)
            dir = -1;
        else if (!_spriteRenderer.flipY)
            dir = 1;

        RaycastHit2D downHit = Physics2D.Raycast(transform.position, Vector2.down, GroundRayLenth, GroundCheck);
        RaycastHit2D upHit = Physics2D.Raycast(transform.position, Vector2.up, CeilingRayLenth, CeilingCheck);
        RaycastHit2D sideHit = Physics2D.Raycast(transform.position, transform.right * dir, WallRayLenth, WallCheck);

        Debug.DrawRay(transform.position, Vector2.down * GroundRayLenth, Color.blue);
        Debug.DrawRay(transform.position, Vector2.up * CeilingRayLenth, Color.blue);
        Debug.DrawRay(transform.position, transform.right * dir * WallRayLenth, Color.blue);

        if (downHit.collider != null)
        {
            bGround = true;
        }
        else
        {
            bGround = false;
        }

        if (upHit.collider != null)
        {
            bCeiling = true;
        }
        else
        {
            bCeiling = false;
        }

        if (sideHit.collider != null)
        {
            bWall = true;
        }
        else
        {
            bWall = false;
        }
    }
}
