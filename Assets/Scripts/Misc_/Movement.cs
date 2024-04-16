using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;

    private float _moveX;
    private bool _canMove = true;

    private Rigidbody2D _rigidbody;
    private Knockback _knockback;
    private PlayerController _playerController;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _knockback = GetComponent<Knockback>();
        _playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        _knockback.OnKnockbackStart += CanMoveFalse;
        _knockback.OnKnockbackEnd += CanMoveTrue;
        _playerController.OnDashLeft += CanMoveFalse;
        _playerController.OnDashRight += CanMoveFalse;
        _playerController.OnDashEnd += CanMoveTrue;
    }

    private void OnDisable()
    {
        _knockback.OnKnockbackStart -= CanMoveFalse;
        _knockback.OnKnockbackEnd -= CanMoveTrue;
        _playerController.OnDashLeft -= CanMoveFalse;
        _playerController.OnDashRight -= CanMoveFalse;
        _playerController.OnDashEnd -= CanMoveTrue;
    }

    private void FixedUpdate()
    { 
        Move();
    }

    void CanMoveTrue()
    {
        _canMove = true;
    }

    void CanMoveFalse()
    {
        _canMove = false;
    }

    public void SetCurrentDirection(float currentDirection)
    {
        _moveX = currentDirection;
    }

    void Move()
    {
        if (!_canMove) return;
        Vector2 movement = new Vector2(_moveX * _moveSpeed, _rigidbody.velocity.y);
        _rigidbody.velocity = movement;
    }
}
