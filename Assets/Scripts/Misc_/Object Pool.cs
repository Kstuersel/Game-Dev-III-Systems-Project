using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    private ObjectPool<Transform> _transformPool;
    public GameObject _prefab;
    void Start()
    {
        CreateTransformPool();
    }

    private void Update()
    {
        Transform transform = _transformPool.Get();
        
        _transformPool.Release(transform);
    }

    void CreateTransformPool()
    {
        /*_transformPool = new ObjectPool<Transform>(() =>
        {
            return Instantiate();//instantiate prefab

        }, transform =>
        {
            transform.gameObject.SetActive(true);
        }, transform =>
        {
            transform.gameObject.SetActive(false);
        }, transform =>
        {
            Destroy(transform);
        }, false, 20, 40);*/
    }
}
