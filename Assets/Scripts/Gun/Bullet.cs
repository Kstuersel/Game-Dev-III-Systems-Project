using System;
using UnityEditorInternal;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 10f;
    [SerializeField] int _damageAmount = 1;
    [SerializeField] float _knockbackFThrust = 4f;

    public GameObject DeathVFX => _deathFXPrefab;
    
    [SerializeField] private GameObject _deathFXPrefab;

    Vector2 _fireDirection;

    Rigidbody2D _rigidBody;
    private Gun _gun;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _rigidBody.velocity = _fireDirection * _moveSpeed;
    }

    public void Init(Vector2 bulletSpawnPos, Vector2 mousePos, Gun gun)
    {
        transform.position = bulletSpawnPos;
        _fireDirection = (mousePos - bulletSpawnPos).normalized;
        float angle = Mathf.Atan2(_fireDirection.y, _fireDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        _gun = gun;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Collider2D[] _hitEnemies = Physics2D.OverlapCircleAll();
        //use rigid body.add torque to make the grenade spin
        
        IHittable iHittable = other.gameObject.GetComponent<IHittable>();
        iHittable?.TakeHit();

        IDamagable iDamageable = other.gameObject.GetComponent<IDamagable>();
        iDamageable?.TakeDamage(_damageAmount, _knockbackFThrust);
        
        Instantiate(DeathVFX, this.transform.position, this.transform.rotation);

        _gun.DisableBullet(this);
    }
}