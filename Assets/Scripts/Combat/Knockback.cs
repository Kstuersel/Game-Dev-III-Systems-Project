using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class Knockback : MonoBehaviour
{
    public Action OnKnockbackStart;
    public Action OnKnockbackEnd;

    [SerializeField] private float _knockbackTime = .2f;

    private Vector3 _hitDirection;
    private float _knockbackThrust;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        OnKnockbackStart += ApplyKnockbackThrust;
        OnKnockbackEnd += StopKnockRoutine;
    }

    private void OnDisable()
    {
        OnKnockbackStart -= ApplyKnockbackThrust;
        OnKnockbackEnd -= StopKnockRoutine;
    }

    public void GetKnockback(Vector3 hitDirection, float knockbackThrust)
    {
        _hitDirection = hitDirection;
        _knockbackThrust = knockbackThrust;

        OnKnockbackStart?.Invoke();
    }

    void ApplyKnockbackThrust()
    {
        Vector3 forceVector = (transform.position - _hitDirection).normalized * _knockbackThrust * _rigidbody.mass;
        _rigidbody.AddForce(forceVector, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }

    IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(_knockbackTime);
        OnKnockbackEnd?.Invoke();
    }

    void StopKnockRoutine()
    {
        _rigidbody.velocity = Vector2.zero;
    }
}
