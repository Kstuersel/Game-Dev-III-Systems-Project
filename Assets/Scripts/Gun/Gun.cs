using UnityEngine;
using System;
using System.Collections;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    public static Action OnShoot;
    public Transform BulletSpawnPoint => _bulletSpawnPoint;

    private ObjectPool<Bullet> _bulletPool;
    private float _bulletTimer = 0f;
    [SerializeField] Transform _bulletSpawnPoint;
    [SerializeField] Bullet _bulletPrefab;
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private float _muzzleFlashTime = .05f;

    private Coroutine _muzzleFlashRoutine;

    private static readonly int FIRE_HASH = Animator.StringToHash("Fire");
    private Vector2 _mousePos;
    private CinemachineImpulseSource _impulseSource;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    
    void Start()
    {
        CreateTransformPool();
    }

    void Update()
    {
        Bullet bullet = _bulletPool.Get();
        Shoot();
        RotateGun();
        _bulletPool.Release(bullet);
    }

    private void OnEnable()
    {
        OnShoot += ShootProjectile; //add listener
        OnShoot += FireAnimation;
        OnShoot += ScreenSHake;
        OnShoot += MuzzleFlash;
    }

    private void OnDisable()
    {
        OnShoot -= ShootProjectile; //remove listener
        OnShoot -= FireAnimation;
        OnShoot -= ScreenSHake;
        OnShoot -= MuzzleFlash;
    }

    void Shoot()
    {
        if (Input.GetMouseButton(0) && _bulletTimer <= 0) 
        {
            OnShoot?.Invoke(); //invoke calls the action
            _bulletTimer = 0.2f;
        }
       else if (_bulletTimer > 0)
        {
            _bulletTimer = (_bulletTimer - Time.deltaTime);
            Debug.Log(_bulletTimer);
        }
    }

    void ScreenSHake()
    {
        _impulseSource.GenerateImpulse();
    }

    void ShootProjectile()
    {
        Bullet newBullet = _bulletPool.Get();
        newBullet.Init(_bulletSpawnPoint.position, _mousePos, this);
    }

    public void DisableBullet(Bullet bullet)
    {
        _bulletPool.Release(bullet);
    }
    void FireAnimation()
    {
        _animator.Play(FIRE_HASH, 0, 0f);
    }

    void RotateGun()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePos);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    void MuzzleFlash()
    {
        if (_muzzleFlashRoutine != null)
        {
            StopCoroutine(_muzzleFlashRoutine);
        }

        _muzzleFlashRoutine = StartCoroutine(MuzzleFlashRoutine());
    }

    IEnumerator MuzzleFlashRoutine()
    {
        _muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(_muzzleFlashTime);
        _muzzleFlash.SetActive(false);
    }
    
    void CreateTransformPool()
    {
        _bulletPool = new ObjectPool<Bullet>(() =>
        {
            return Instantiate(_bulletPrefab);//instantiate prefab

        }, transform =>
        {
            transform.gameObject.SetActive(true);
        }, transform =>
        {
            transform.gameObject.SetActive(false);
        }, transform =>
        {
            Destroy(transform);
        }, false, 10, 15);
    }
}
