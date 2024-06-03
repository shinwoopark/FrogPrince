using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{

    public GameObject Tongue;

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;

    private float _gravityScale;
    private float _direction;

    public float FowardPower;

    public float JumpPower;
    private float _jumpTime;

    public float DashPower;
    private float _dashCoolTime;
    private float _dashTime;
    private float _dashDirection;

    public float ChargeJumpPower;
    public float ChargeSideJumpPower;
    private float _chargeJumpPower;
    private float _chargeSideJumpPower;
    private float _chargeJumpTime;

    public float ClimbPower;
    private float _climbDirection;

    public float TonguePower;
    public float TongueLenth;

    private bool _bGround;

    private bool _bHitwall;

    private bool _bCeiling;

    private bool _bFoward;

    private bool _bJump;
    private bool _bJumping;

    private bool _bDash;
    private bool _bDashing;

    private bool _bChargeJump;
    private bool _bChargeJumping;

    private bool _bSlide;

    private bool _bClimb;
    private bool _bClimbing;
    private bool _bExitClimb;

    private bool _bMoveTongue;
    private bool _bSizeUpTongue;
    private bool _bSizeDownTongue;
    private bool _bFollowTongue;

    private string _sHitTongue;

    //Ray
    public float GroundRayLenth, CeilingRayLenth, WallRayLenth;
    public LayerMask GroundCheck, CeilingCheck, WallCheck;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _direction = -1;
    }

    private void Update()
    {
        UpdateRaycast();
        UpdateState();
        InputMove();
        UpdateGravitySet();
        UpdateChargeJumpFinish();
    }

    private void FixedUpdate()
    {
        UpdateGravity();
        UpdateFoward();
        UpdateJump();
        UpdateDash();
        UpdateChargeJump();
        UpdateClimb();
        UpdateMoveTongue();
    }

    private void InputMove()
    {
        //Forward
        if (!_bDashing && !_bChargeJump && !_bChargeJumping && !_bClimb && !_bMoveTongue
            && Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.D))
        {
            _bFoward = true;
            _spriteRenderer.flipY = false;
            _direction = -1;
        }
        else if (!_bDashing && !_bChargeJump && !_bChargeJumping && !_bClimb && !_bMoveTongue
            && Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.A))
        {
            _bFoward = true;
            _spriteRenderer.flipY = true;
            _direction = 1;
        }
        else
        {
            _bFoward = false;
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && !_bMoveTongue)
        {
            if (_bJump && !_bDashing && !_bChargeJump && !_bClimbing)
            {
                _bJumping = true;
            }
            else if (_bClimbing)
            {
                _bExitClimb = true;
            }
        }

        //Dash
        if (Input.GetKeyDown(KeyCode.X)
            && _bDash && !_bChargeJump && !_bHitwall && !_bMoveTongue
            && _dashCoolTime <= 0)
        {
            _bJumping = false;
            _bChargeJumping = false;
            _bDashing = true;
            _dashDirection = _direction;
            _dashCoolTime = 1;
        }

        //JumpCharge
        if (GameInstance.instance.TrasformLevel >= 1
            && Input.GetKeyDown(KeyCode.DownArrow)
            && _bJump && !_bDashing && !_bClimb && !_bMoveTongue)
        {
            _bChargeJump = true;
        }

        //Climb
        if (_bClimb && !_bMoveTongue)
        {
            _bClimbing = true;

            if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
            {
                _climbDirection = 1;
            }
            else if (Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow))
            {
                _climbDirection = -1;
            }
            else
            {
                _climbDirection = 0;
            }
        }
        else
        {
            _bClimbing = false;
        }

        //Tongue
        if (Input.GetKeyDown(KeyCode.C) && !_bDashing && !_bMoveTongue)
        {
            Tongue.SetActive(true);

            if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
                {
                    Tongue.transform.eulerAngles = new Vector3(0, 0, 45);
                }
                else if (!Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow))
                {
                    Tongue.transform.eulerAngles = new Vector3(0, 0, 135);
                }
                else
                {
                    Tongue.transform.eulerAngles = new Vector3(0, 0, 90);
                }
            }
            else if (!Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
            {
                if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
                {
                    Tongue.transform.eulerAngles = new Vector3(0, 0, -45);
                }
                else if (!Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow))
                {
                    Tongue.transform.eulerAngles = new Vector3(0, 0, -135);
                }
                else
                {
                    Tongue.transform.eulerAngles = new Vector3(0, 0, -90);
                }
            }
            else if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
            {
                Tongue.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (!Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow))
            {
                Tongue.transform.eulerAngles = new Vector3(0, 0, 180);
            }
            else
            {
                Tongue.transform.eulerAngles = new Vector3(0, 0, _direction * -90);
            }

            _bMoveTongue = true;
            _bSizeUpTongue = true;
        }
    }

    private void UpdateRaycast()
    {
        RaycastHit2D downHit = Physics2D.Raycast(transform.position, Vector2.down, GroundRayLenth, GroundCheck);
        RaycastHit2D upHit = Physics2D.Raycast(transform.position, Vector2.up, CeilingRayLenth, CeilingCheck);
        RaycastHit2D sideHit = Physics2D.Raycast(transform.position, Vector2.right * _direction, WallRayLenth, WallCheck);

        Debug.DrawRay(transform.position, Vector2.down * GroundRayLenth, Color.blue);
        Debug.DrawRay(transform.position, Vector2.up * CeilingRayLenth, Color.blue);
        Debug.DrawRay(transform.position, Vector2.right * _direction * WallRayLenth, Color.blue);

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
            _bHitwall = true;
        }
        else
        {
            _bHitwall = false;
        }

        //EnemyHit
        RaycastHit2D enemyHit;

        if(Tongue != null)
        {
            float angleStep = 360.0f / Tongue.transform.rotation.z;
            float radian = angleStep * Mathf.Deg2Rad;

            Vector3 direction = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0);

            enemyHit = Physics2D.Raycast(Tongue.transform.position, direction, GroundRayLenth, GroundCheck);

            Debug.DrawRay(Tongue.transform.position, direction, Color.green);
        }
    }

    private void UpdateState()
    {
        //bGround
        if (_bGround)
        {
            _bJump = true;
            _bDash = true;
        }
        else
        {
            _bJump = false;
        }

        //bCeiling
        if (_bCeiling)
        {
            _bJumping = false;
            _bExitClimb = false;
            _bChargeJumping = false;
        }

        //bHitWall
        if (_bHitwall)
        {
            if (GameInstance.instance.TrasformLevel == 1 && !_bJumping)
            {
                if (!_bGround)
                {
                    _bSlide = true;
                }
            }

            if (GameInstance.instance.TrasformLevel == 2)
            {
                if (!_bGround)
                {
                    _bJumping = false;
                    _bClimb = true;
                    _bDash = true;
                }
            }
        }
        else
        {
            _bClimb = false;
        }
    }

    private void UpdateGravitySet()
    {
        if (_bJumping || _bDashing || _bChargeJumping || _bClimb || _bMoveTongue)
        {
            _gravityScale = 0;
        }
        else if (_bSlide)
        {
            _gravityScale = 3.75f;
        }
        else
        {
            _gravityScale = 7.5f;
        }
    }

    private void UpdateGravity()
    {
        if (!_bGround)
        {
            transform.position += new Vector3(0, -_gravityScale * Time.deltaTime, 0);
        }
    }

    private void UpdateFoward()
    {
        if (_bFoward)
        {
            if (!_bHitwall)
                transform.position += new Vector3(_direction * FowardPower * Time.deltaTime, 0, 0);
        }
    }

    private void UpdateJump()
    {
        if (_bJumping && Input.GetKey(KeyCode.Space) && _jumpTime > 0)
        {
            _jumpTime -= Time.deltaTime;
            transform.position += new Vector3(0, JumpPower * Time.deltaTime, 0);
        }
        else
        {
            _bJumping = false;
        }

        if (_bExitClimb && Input.GetKey(KeyCode.Space) && _jumpTime > 0)
        {
            _jumpTime -= Time.deltaTime;
            transform.position += new Vector3(JumpPower, JumpPower, 0) * Time.deltaTime;
        }
        else
        {
            _bExitClimb = false;
        }

        if (!_bJumping && !_bExitClimb)
        {
            _jumpTime = 0.25f;
        }
    }

    private void UpdateDash()
    {
        if (_bDashing && _dashTime > 0)
        {
            _dashTime -= Time.deltaTime;
            transform.position += new Vector3(_dashDirection * DashPower * Time.deltaTime, 0, 0);
            _bDash = false;
        }
        else
        {
            _bDashing = false;
            _dashTime = 0.1f;
            _dashCoolTime -= Time.deltaTime;
        }
    }

    private void UpdateChargeJump()
    {
        if (_bChargeJump && !_bMoveTongue && Input.GetKey(KeyCode.DownArrow))
        {
            _chargeJumpTime += Time.deltaTime * 0.25f;

            if (_chargeJumpTime >= 0.5f)
            {
                _chargeJumpTime = 0.5f;
            }
        }

        if (_bChargeJumping && _chargeJumpTime > 0 && !_bHitwall && !_bMoveTongue)
        {
            _chargeJumpTime -= Time.deltaTime;
            transform.position += new Vector3(_chargeSideJumpPower * Time.deltaTime, _chargeJumpPower * Time.deltaTime, 0);
        }
        else
        {
            _bChargeJumping = false;
        }
    }

    private void UpdateChargeJumpFinish()
    {
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                _chargeJumpPower = ChargeJumpPower;
                _chargeSideJumpPower = ChargeSideJumpPower;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                _chargeJumpPower = ChargeJumpPower;
                _chargeSideJumpPower = -ChargeSideJumpPower;
            }
            else
            {
                _chargeJumpPower = ChargeJumpPower;
                _chargeSideJumpPower = 0;
            }

            _bChargeJump = false;
            _bChargeJumping = true;
        }
    }

    private void UpdateClimb()
    {
        if (_bClimbing)
        {
            if (_bCeiling && _climbDirection == 1)
            {

            }
            else
            {
                transform.position += new Vector3(0, ClimbPower * _climbDirection * Time.deltaTime, 0);
            }

            //ExitClimb
            if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && _direction == -1)
            {
                transform.position += new Vector3(FowardPower * Time.deltaTime, 0, 0);
            }
            else if (!Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow) && _direction == 1)
            {
                transform.position += new Vector3(-FowardPower * Time.deltaTime, 0, 0);
            }
            //
        }
    }

    private void UpdateMoveTongue()
    {
        if (_bSizeUpTongue)
        {
            Tongue.transform.localScale += new Vector3(0, TonguePower * Time.deltaTime, 0);

            if (Tongue.transform.localScale.y >= TongueLenth)
            {
                HitTongue("none");
            }
        }

        if (_bSizeDownTongue)
        {
            Tongue.transform.localScale -= new Vector3(0, TonguePower * Time.deltaTime, 0);

            //MovePlayer
            if (_bFollowTongue && _sHitTongue != "none")
            {
                Debug.Log(Tongue.transform.eulerAngles.z);
                if (_sHitTongue == "platform")
                {
                    if (_bCeiling || _bHitwall)
                    {
                        _bFollowTongue = false;
                    }
                }

                if (_sHitTongue == "Medium" || _sHitTongue == "Large")
                {

                }

                switch (Tongue.transform.eulerAngles.z)
                {
                    case 0:
                        transform.position += new Vector3(0, TonguePower * 0.25f, 0) * Time.deltaTime;
                        Debug.Log(Tongue.transform.eulerAngles.z);
                        break;
                    case 45:
                        transform.position += new Vector3(-TonguePower * 0.25f, TonguePower * 0.25f, 0) * Time.deltaTime;
                        Debug.Log(Tongue.transform.eulerAngles.z);
                        break;
                    case 90:
                        transform.position += new Vector3(-TonguePower * 0.25f, 0, 0) * Time.deltaTime;
                        Debug.Log(Tongue.transform.eulerAngles.z);
                        break;
                    case 135:
                        transform.position += new Vector3(-TonguePower * 0.25f, -TonguePower * 0.25f, 0) * Time.deltaTime;
                        Debug.Log(Tongue.transform.eulerAngles.z);
                        break;
                    case 180:
                        transform.position += new Vector3(0, -TonguePower * 0.25f, 0) * Time.deltaTime;
                        Debug.Log(Tongue.transform.eulerAngles.z);
                        break;
                    case -135:
                        transform.position += new Vector3(TonguePower * 0.25f, -TonguePower * 0.25f, 0) * Time.deltaTime;
                        Debug.Log(Tongue.transform.eulerAngles.z);
                        break;
                    case -90:
                        transform.position += new Vector3(TonguePower * 0.25f, 0, 0) * Time.deltaTime;
                        Debug.Log(Tongue.transform.eulerAngles.z);
                        break;
                    case -45:
                        transform.position += new Vector3(TonguePower * 0.25f, TonguePower * 0.25f, 0) * Time.deltaTime;
                        Debug.Log(Tongue.transform.eulerAngles.z);
                        break;
                }
            }
            //

            if (Tongue.transform.localScale.y <= 0)
            {
                Tongue.transform.localScale = new Vector3(0.25f, 0, 1);
                Tongue.SetActive(false);
                _bSizeDownTongue = false;
                _bMoveTongue = false;
            }
        }
    }

    public void HitTongue(string hit)
    {
        _sHitTongue = hit;
        _bSizeUpTongue = false;
        _bFollowTongue = true;
        _bSizeDownTongue = true;
    }

    private void KnockBack()
    {

    }
}
