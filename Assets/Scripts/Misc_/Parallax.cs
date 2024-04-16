using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralaxx : MonoBehaviour
{
    [SerializeField] private float _parallaxOffset = .9f;

    private Vector2 _startPos;
    private Camera _mainCamera;
    private Vector2 _travel => (Vector2)_mainCamera.transform.position - _startPos;

    void Awake()
    {
        _mainCamera = Camera.main;
    }
    void Start()
    {
        _startPos = transform.position;
    }

    private void FixedUpdate()
    {
        Vector2 newPosition = _startPos + new Vector2(_travel.x * _parallaxOffset, 0f);
        transform.position = (Vector2)new Vector2(newPosition.x, transform.position.y);
    }
}
