using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerAnimations : MonoBehaviour
{

    [SerializeField] private float _yLandVelocityCheck = -10f;

    private Vector2 _velocityBeforePhysicsUpdate;
    private Rigidbody2D _rigidbody;
    private CinemachineImpulseSource _impulseSource;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void FixedUpdate()
    {
        _velocityBeforePhysicsUpdate = _rigidbody.velocity;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (_velocityBeforePhysicsUpdate.y < _yLandVelocityCheck)
        {
            _impulseSource.GenerateImpulse();
        }
    }
}
