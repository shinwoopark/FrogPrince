using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerControll : MonoBehaviour
{
    //대쉬, 행동(혀 등)
    private SpriteRenderer _spriteRenderer;

    private PlayerState _playerState;

    public Transform AttackPos;
    public Vector2 AttackSize;
    public LayerMask HitLayers;
    public float AttackCoolTime;
    private float AttackCurrentTime = 1;

    public float DashPower;
    private float _dashCoolTime;
    private float _dashTime;
    private float _dashDirection;

    private void Awake()
    {
        _playerState = GetComponent<PlayerState>();
        _spriteRenderer = GetComponent<SpriteRenderer>();      
    }

    private void Update()
    {
        InputControll();
        UpdateTongueDir();
    }

    private void FixedUpdate()
    {
        UpdateDash();
        UpdateMoveTongue();
    }

    private void InputControll()
    {
        //Attack
        AttackCurrentTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Z) && AttackCurrentTime >= AttackCoolTime
            && _playerState.CurrentState != PlayerStates.Dash
            && _playerState.CurrentState != PlayerStates.MoveTongue)
        {
            if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
            {
                AttackPos.position = transform.localPosition + new Vector3(0, 1, 0);
                //AttackCoolTime = 1;
            }
            else if(!Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow))
            {
                AttackPos.position = transform.localPosition + new Vector3(0, -1, 0);
                //AttackCoolTime = 1;
            }
            else
            {
                if (_spriteRenderer.flipY)
                {
                    AttackPos.position = transform.localPosition + new Vector3(-1, 0, 0);
                }
                else if (!_spriteRenderer.flipY)
                {
                    AttackPos.position = transform.localPosition + new Vector3(1, 0, 0);
                }
            }

            StartCoroutine(Attack());
            AttackCurrentTime = 0;   
        }

        //Dash
        if (Input.GetKeyDown(KeyCode.X)
            && _playerState.bDash
            && _dashCoolTime <= 0
            && _playerState.CurrentState != PlayerStates.Slide
            && _playerState.CurrentState != PlayerStates.Climb
            && _playerState.CurrentState != PlayerStates.MoveTongue)
        {
            if (_spriteRenderer.flipY)
                _dashDirection = -1;
            else if (!_spriteRenderer.flipY)
                _dashDirection = 1;

            _playerState.CurrentState = PlayerStates.Dash;
            _dashCoolTime = 1;
        }

        //Tongue
        //if(Input.GetKeyDown(KeyCode.C)
        //    && _playerState.CurrentState != State.Dash
        //    && _playerState.CurrentState != State.MoveTongue)
        //{
        //    _playerState.CurrentState = State.MoveTongue;
        //}
    }

    IEnumerator Attack()
    {
        _playerState.CurrentState = PlayerStates.Attack;

        Collider2D[] AttackBox = Physics2D.OverlapBoxAll(AttackPos.position, AttackSize, 0, HitLayers);

        foreach(Collider2D hit in AttackBox)
        {
            Debug.Log(hit);
        }

        yield return new WaitForSeconds(0.25f);

        _playerState.CurrentState = PlayerStates.Idle;
    }

    private void UpdateDash()
    {
        if (_playerState.CurrentState == PlayerStates.Dash)
        {
            _dashTime -= Time.deltaTime;

            transform.position += new Vector3(_dashDirection * DashPower * Time.deltaTime, 0, 0);

            if (_playerState.CurrentState != PlayerStates.Dash || _dashTime <= 0)
            {
                _playerState.CurrentState = PlayerStates.Idle;
                _playerState.bDash = false;
            }
        }
        else
        {
            _dashTime = 0.1f;
            _dashCoolTime -= Time.deltaTime;
        }
    }

    private void UpdateTongueDir()
    {

    }

    private void UpdateMoveTongue()
    {
        if (_playerState.CurrentState == PlayerStates.MoveTongue)
        {

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(AttackPos.position, AttackSize);
    }
}
