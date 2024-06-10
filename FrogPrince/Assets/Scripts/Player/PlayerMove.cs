using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMove : MonoBehaviour
{
    //이동, 점프, 차지점프, 등반

    private PlayerState _playerState;

    private SpriteRenderer _spriteRenderer;

    public float MoveSpeed;
    private int _moveDir = 0;

    public float JumpPower;
    private float _jumpTime;

    public float ChargeJumpPower;
    private float _chargeJumpTime;
    private float _chargeJumpMaxTime = 2;

    public float ClimbPower;
    private int _climbDir = 0;

    void Awake()
    {
        _playerState = GetComponent<PlayerState>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        InputMove();
    }

    private void FixedUpdate()
    {
        UpdateFoward();
        UpdateJump();
        UpdateChargeJump();
        UpdateClimb();
    }

    private void InputMove()
    {
        _moveDir = 0;
        if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            if(_playerState.bWall && !_spriteRenderer.flipY)
            {
                return;
            }
            else
            {
                _moveDir = 1;
                _spriteRenderer.flipY = false;
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            if (_playerState.bWall && _spriteRenderer.flipY)
            {
                return;
            }
            else
            {
                _moveDir = -1;
                _spriteRenderer.flipY = true;
            }           
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && _playerState.bGround)
        {
            if(_playerState.CurrentState == PlayerStates.Idle)
            {
                _playerState.CurrentState = PlayerStates.Jumping;
            }
            else if(_playerState.CurrentState == PlayerStates.Climb)
            {

            }
        }

        //ChargeJump
        if (GameInstance.instance.TrasformLevel >= 1) 
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) && _playerState.CurrentState == PlayerStates.Idle && _playerState.bGround)
            {
                _playerState.CurrentState = PlayerStates.Charging;
            }

            if (Input.GetKey(KeyCode.DownArrow) && _playerState.CurrentState == PlayerStates.Charging)
            {
                if (_chargeJumpMaxTime > _chargeJumpTime)
                {
                    _chargeJumpTime += Time.deltaTime;
                }           
            }

            if (Input.GetKeyUp(KeyCode.DownArrow) && _playerState.CurrentState == PlayerStates.Charging)
            {
                _playerState.CurrentState = PlayerStates.ChargeJumping;
            }
        }
        
        //Climb
        if(_playerState.CurrentState == PlayerStates.Climb)
        {
            if(Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow))
            {
                _climbDir = -1;
            }
            else if(!Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.UpArrow))
            {
                _climbDir = 1;
            }
            else
            {
                _climbDir = 0;
            }
        }
    }

    private void UpdateFoward()
    {
        if(_playerState.CurrentState != PlayerStates.Charging
            && _playerState.CurrentState != PlayerStates.Dash
            && _playerState.CurrentState != PlayerStates.MoveTongue)
        {
            transform.position += new Vector3(_moveDir * MoveSpeed * Time.deltaTime, 0, 0);
        }       
    }

    private void UpdateJump()
    {
        if (_playerState.CurrentState == PlayerStates.Jumping)
        {
            if (Input.GetKey(KeyCode.Space) && _jumpTime > 0)
            {
                _jumpTime -= Time.deltaTime;

                transform.position += new Vector3(0, JumpPower * Time.deltaTime, 0);
            }
            else
            {
                _playerState.CurrentState = PlayerStates.Idle;
            }
        }
        else
        {
            _jumpTime = 0.25f;
        }

        //if (_bExitClimb && Input.GetKey(KeyCode.Space) && _jumpTime > 0)
        //{
        //    _jumpTime -= Time.deltaTime;
        //    transform.position += new Vector3(JumpPower, JumpPower, 0) * Time.deltaTime;
        //}
        //else
        //{
        //    _bExitClimb = false;
        //}

        //if (!_bJumping && !_bExitClimb)
        //{
            
        //}
    }

    private void UpdateChargeJump()
    {
        if (_playerState.CurrentState == PlayerStates.ChargeJumping)
        {
            if (_chargeJumpTime > 0)
            {
                _chargeJumpTime -= Time.deltaTime * 5;

                transform.position += new Vector3(0, ChargeJumpPower * Time.deltaTime, 0);
            }
            else
            {
                _playerState.CurrentState = PlayerStates.Idle;
            }
        }
    }

    private void UpdateClimb()
    {
        if (_playerState.CurrentState == PlayerStates.Climb)
        {
            transform.position += new Vector3(0, ClimbPower * _climbDir * Time.deltaTime, 0);
        }
    }
}
