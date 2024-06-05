using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMove : MonoBehaviour
{
    //이동, 점프, 차지점프, 등반

    private Rigidbody2D _rigid;
    private SpriteRenderer _spriteRenderer;
    private PlayerState _playerState;

    public float MoveSpeed = 5;
    private int _moveDir = 0;

    public float JumpPower;
    private float _jumpTime;

    public float ChargeJumpPower;
    private float _chargeJumpTime;

    void Awake()
    {
        _playerState = GetComponent<PlayerState>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        InputMove();
    }

    private void FixedUpdate()
    {
        UpdateFoward();
        UpdateJump();
    }

    private void InputMove()
    {
        _moveDir = 0;
        if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            _moveDir = 1;
            _spriteRenderer.flipY = false;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            _moveDir = -1;
            _spriteRenderer.flipY = true;

        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(_playerState.CurrentState == State.Idle)
            {
                _playerState.CurrentState = State.Jumping;
            }
            else if(_playerState.CurrentState == State.Climb)
            {

            }
        }

        //ChargeJump
        //Getkey랑 time.deltatime으로 점프 시간 체크
        //일정 이상이면 누른만큼 float 변수에 더해서 jumpforce 더하기
        //getkeyUDown에 state 변경, getkeyUp일 때 state를 idle로
        if (Input.GetKey(KeyCode.DownArrow) && _playerState.CurrentState == State.Idle)
        {
            _playerState.CurrentState = State.Charging;
            _chargeJumpTime += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.DownArrow) && _playerState.CurrentState == State.Idle)
        {
            _playerState.CurrentState = State.ChargeJumping;
        }
    }

    private void UpdateFoward()
    {
        transform.position += new Vector3(_moveDir * MoveSpeed * Time.deltaTime, 0, 0);
    }

    private void UpdateJump()
    {
        if (_playerState.CurrentState == State.Jumping)
        {
            if (Input.GetKey(KeyCode.Space) && _jumpTime > 0)
            {
                _jumpTime -= Time.deltaTime;

                transform.position += new Vector3(0, JumpPower * Time.deltaTime, 0);
            }
            else
            {
                _playerState.CurrentState = State.Idle;
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
        if (_playerState.CurrentState == State.ChargeJumping)
        {
            if (_chargeJumpTime > 0)
            {
                _chargeJumpTime -= Time.deltaTime;

                transform.position += new Vector3(0, ChargeJumpPower * Time.deltaTime, 0);
            }
            else
            {
                _jumpTime = 0.25f;
            }
        }
    }
}
