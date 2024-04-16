using System;
using TMPro.SpriteAssetUtilities;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public static Action OnJump;
    public Action OnDashLeft;
    public Action OnDashRight;
    public Action OnDashEnd;
    public static PlayerController Instance;
    
    [SerializeField] float _jumpStrength = 7f;
    [SerializeField] private Transform _footTransform;
    [SerializeField] Vector2 _groundCheck;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] private float _extraGravity = 900f;
    [SerializeField] private float _maxFallSpeed = -25f;
    [SerializeField] private float _gravityDelay = 0.2f;
    [SerializeField] private float _coyoteTime = .5f;
    [SerializeField] private float _dashTimer = 1f;
    [SerializeField] private float _dashRechargeTimer = 1f;
    [SerializeField] private float _dashStrength = 5f;

    private PlayerInput _playerInput;
    private FrameInput _frameInput;

    private float _timeInAir, _coyoteTimer;
    private bool _doubleJumpAvailable;

    Rigidbody2D _rigidBody;
    Animator _animator;
    private Movement _movement;

    public void Awake() {
        if (Instance == null) { Instance = this; }

        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _movement = GetComponent<Movement>();
    }

    private void OnEnable()
    {
        OnJump += ApplyJumpFOrce;
        OnDashLeft += ApplyDashForceLeft;
        OnDashRight += ApplyDashForceRight;
        OnDashEnd += StopDashRoutine;
    }

    private void OnDisable()
    {
        OnJump -= ApplyJumpFOrce;
        OnDashLeft -= ApplyDashForceLeft;
        OnDashRight -= ApplyDashForceRight;
        OnDashEnd -= StopDashRoutine;
    }

    void Update()
    {
        GatherInput();
        Move();
        CoyoteTimer();
        DashTimer();
        HandleJump();
        HandleDashLeft();
        HandleDashRight();
        HandleSpriteFlip();
        GravityDelay();
    }

    private void FixedUpdate()
    {
       ExtraGravity();
    }

    public bool IsFacingRight()
    {
        return transform.eulerAngles.y == 0;
    }

    bool CheckGrounded()
    {
        Collider2D _isGrounded = Physics2D.OverlapBox(_footTransform.position, _groundCheck, 0f, _groundLayer);
        return _isGrounded;
    }

    void GatherInput()
    {
        _frameInput = _playerInput.FrameInput;
    }

    void Move()
    {
        _movement.SetCurrentDirection(_frameInput.Move.x);
        
        if(Mathf.Abs(_rigidBody.velocity.x) < Mathf.Epsilon)
            _animator.SetBool("Running", false);
        else
            _animator.SetBool("Running", true); 
        
        if (_rigidBody.velocity.y < 0)
        {
            _animator.SetBool("Running", false);
            _animator.SetBool("Falling", true);
        }
        else if (_rigidBody.velocity.y >= 0)
        {
            _animator.SetBool("Falling", false);
        }
    }

    void HandleJump()
    {
        if (!_frameInput.Jump) return;
        
        if (CheckGrounded() || _coyoteTimer > 0f)
        {
            _doubleJumpAvailable = true;
            _coyoteTimer = 0f;
            OnJump?.Invoke();
        }
        else if (_doubleJumpAvailable)
        {
            _doubleJumpAvailable = false;
            OnJump?.Invoke();
        }
    }

    void HandleDashLeft()
    {
        if (!_frameInput.DashLeft) return;

        if (_dashRechargeTimer >= 1f)
        {
            _dashRechargeTimer = 0f;
            OnDashLeft?.Invoke();
        }
    }

    void HandleDashRight()
    {
        if (!_frameInput.DashRight) return;
        
        if (_dashRechargeTimer >= 1f)
        {
            _dashRechargeTimer = 0f;
            OnDashRight?.Invoke();
        }
    }

    void CoyoteTimer()
    {
        if (CheckGrounded())
        {
            _coyoteTimer = _coyoteTime;
            _doubleJumpAvailable = true;
        }
        else
        {
            _coyoteTimer -= Time.deltaTime;
        }
    }

    void DashTimer()
    {
        _dashRechargeTimer += Time.deltaTime;
    }

    void ApplyJumpFOrce()
    {
        _rigidBody.velocity = Vector2.zero;
        _timeInAir = 0;
        _coyoteTimer = 0f;
        _rigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);
    }

    void ApplyDashForceLeft()
    {
        _rigidBody.AddForce(Vector2.left * _dashStrength, ForceMode2D.Impulse);
        StartCoroutine(DashRoutine());
    }
    
    void ApplyDashForceRight()
    {
        _rigidBody.AddForce(Vector2.right * _dashStrength, ForceMode2D.Impulse);
        StartCoroutine(DashRoutine());
    }

    IEnumerator DashRoutine()
    {
        yield return new WaitForSeconds(_dashTimer);
        OnDashEnd?.Invoke();
    }

    void StopDashRoutine()
    {
        _rigidBody.velocity = Vector2.zero;
    }

    void HandleSpriteFlip()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }
    void GravityDelay()
         {
             if (!CheckGrounded())
             {
                 _timeInAir += Time.deltaTime;
             }
             else
             {
                 _timeInAir = 0;
             }
         }

    void ExtraGravity()
    {
        if (_timeInAir >= _gravityDelay)
        {
            _rigidBody.AddForce(new Vector2(0f, -_extraGravity * Time.deltaTime));
            if (_rigidBody.velocity.y < _maxFallSpeed)
            {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _maxFallSpeed);
            }
        }
    }
    void OnDrawGizmos()
      {
          Gizmos.color = Color.red;
          Gizmos.DrawWireCube(_footTransform.position, _groundCheck);
      }

    
}


