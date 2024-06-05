using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum State
{
    Idle,
    Jumping,
    Charging,
    ChargeJumping,
    Dash,
    Climb,
    MoveTongue
}

public class PlayerState : MonoBehaviour
{
    public State CurrentState = State.Idle;

    public float GroundRayLenth, CeilingRayLenth, WallRayLenth;
    public LayerMask GroundCheck, CeilingCheck, WallCheck;

    private bool _bGround;

    private bool _bWall;

    private bool _bCeiling;

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    private void UpdateRayCast()
    {
        RaycastHit2D downHit = Physics2D.Raycast(transform.position, Vector2.down, GroundRayLenth, GroundCheck);
        RaycastHit2D upHit = Physics2D.Raycast(transform.position, Vector2.up, CeilingRayLenth, CeilingCheck);
        RaycastHit2D sideHit = Physics2D.Raycast(transform.position, transform.right, WallRayLenth, WallCheck);

        Debug.DrawRay(transform.position, Vector2.down * GroundRayLenth, Color.blue);
        Debug.DrawRay(transform.position, Vector2.up * CeilingRayLenth, Color.blue);
        Debug.DrawRay(transform.position, transform.right * WallRayLenth, Color.blue);

        if (downHit.collider != null)
        {
            _bGround = true;
        }
        else
        {
            _bGround = false;
        }

        if (upHit.collider != null)
        {
            _bCeiling = true;
        }
        else
        {
            _bCeiling = false;
        }

        if (sideHit.collider != null)
        {
            _bWall = true;
        }
        else
        {
            _bWall = false;
        }
    }
}
