using UnityEngine;
using Zenject;
using System;


public class Bullet : MonoBehaviour, IPoolable<Vector2, float>
{
    public float speed;
    public float gravity;
    public float initialAngle;
    private Vector2 _direction;
    private IMemoryPool _pool;
    private Rigidbody2D _rb;
    public static event Action<IEnemy> EnemyHit;
    [Inject]
    public void Construct(Bullet.Pool pool)
    {
        _pool = pool;
        _rb = GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().material = new Material(GetComponent<SpriteRenderer>().material);
    }

    public void OnSpawned(Vector2 direction, float chargeTime)
    {
        _direction = Quaternion.Euler(0, 0, (direction.x >= 0 ? 1f : -1f) * initialAngle ) * direction.normalized;
        _rb.velocity = _direction * speed* (0.3f+ 3f*chargeTime);
        gameObject.SetActive(true);
    }
    public void OnDespawned()
    {
        _rb.velocity = Vector2.zero;
    }
    void FixedUpdate()
    {
        _rb.velocity += Vector2.down * gravity;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Level"))
        {
            _pool.Despawn(this);
        }else  if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
            EnemyHit?.Invoke(enemy);
            _pool.Despawn(this);
        }//TODO - mark already queued for despawn
    }

    public class Pool : MonoMemoryPool<Vector2, float, Bullet>
    {
        protected override void Reinitialize(Vector2 direction,float chargeTime,  Bullet bullet)
        {
            bullet.OnSpawned(direction, chargeTime);
        }
    }
}