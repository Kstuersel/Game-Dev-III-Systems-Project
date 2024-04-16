using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour , IDamagable
{
    [SerializeField] float _jumpForce = 7f;
    [SerializeField] float _jumpInterval = 4f;
    [SerializeField] float _changeDirectionInterval = 3f;

    Rigidbody2D _rigidBody;
    private Movement _movement;
    private ColorChanger _colorChanger;
    private Knockback _knockback;
    private Flash _flash;
    private Health _health;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _movement = GetComponent<Movement>();
        _colorChanger = GetComponent<ColorChanger>();
        _knockback = GetComponent<Knockback>();
        _flash = GetComponent<Flash>();
        _health = GetComponent<Health>();
    }

    void Start()
    {
        Init();
        StartCoroutine(ChangeDirection());
        StartCoroutine(RandomJump());
    }

     void Init()
    {
        _colorChanger.SetRandomColor();
    }

    IEnumerator ChangeDirection()
    {
        while (true)
        {
            float currentDirection = Random.Range(0, 2) * 2 - 1; // 1 or -1
            _movement.SetCurrentDirection(currentDirection);
            yield return new WaitForSeconds(_changeDirectionInterval);
        }
    }

    IEnumerator RandomJump() 
    {
        while (true)
        {
            yield return new WaitForSeconds(_jumpInterval);
            float randomDirection = Random.Range(-1, 1);
            Vector2 jumpDirection = new Vector2(randomDirection, 1f).normalized;
            _rigidBody.AddForce(jumpDirection * _jumpForce, ForceMode2D.Impulse);
        }
    }

    public void TakeDamage(int damageAmount, float knockbackThrust)
    {
        _health.TakeDamage(damageAmount);
        
        _knockback.GetKnockback(PlayerController.Instance.transform.position, knockbackThrust);
    }

    public void TakeHit()
    {
        _flash.StartFlash();
    }
}
