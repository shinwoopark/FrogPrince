using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Jumping,
    Charging,
    ChargeJumping,
    Dash,
    Slide,
    Climb,
    Attack,
    MoveTongue
}

public class PlayerStateSystem : MonoBehaviour
{
    public PlayerState CurrentState = PlayerState.Idle;

    private SpriteRenderer _spriteRenderer;

    public float GroundRayLenth, CeilingRayLenth, WallRayLenth;
    public LayerMask GroundCheck, CeilingCheck, WallCheck;

    public bool bGround;

    public bool bWall;

    public bool bCeiling;

    private float _gravityScale;

    public bool bDash;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        UpdateGravitySet();
        UpdateRayCast();
        UpdateHitWall();
    }

    private void FixedUpdate()
    {
        UpdateGravity();
    }

    private void UpdateGravitySet()
    {
        if(!bGround)
        {
            if(CurrentState == PlayerState.Idle
                || CurrentState == PlayerState.Attack)
            {
                _gravityScale = 7.5f;
            }          
        }
        else if (CurrentState == PlayerState.Slide)
        {
            _gravityScale = 3.75f;
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
            bDash = true;
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

    private void UpdateHitWall()
    {
        if (GameInstance.instance.TrasformLevel == 1)
        {
            if (bWall)
            {
                CurrentState = PlayerState.Slide;
            }

            if (CurrentState == PlayerState.Slide)
            {
                if (!bWall)
                    CurrentState = PlayerState.Idle;
            }
        }
        else if (GameInstance.instance.TrasformLevel >= 2)
        {
            if (bWall)
            {
                CurrentState = PlayerState.Climb;
            }

            if (CurrentState == PlayerState.Climb)
            {
                bDash = true;

                if (!bWall)
                    CurrentState = PlayerState.Idle;
            }
        }
    }
}
