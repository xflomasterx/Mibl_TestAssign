using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour, IEnemy
{
    public event System.Action OnPlayerTouched;
    private EnemyType _enemyType;
    private PlayerController _playerController;
    private Rigidbody2D _rb;
    private Vector3 _pointA;
    private Vector3 _pointB;
    private Vector3 _originalPosition;
    private Vector3 _currentTarget;
    private bool _chasing = false;
    private bool _isInited = false;
    private bool _currentPatroolPoint = false;
    private float _targetReachingTime = 0;
    private float _targetReachingTimeLimit = 4f;
    private float _initialScaleX;

    public void Init(EnemyType enemyType, PlayerController playerController)
    {
        _playerController = playerController;
        _enemyType = enemyType;
        _originalPosition = transform.position;
        _pointA = _originalPosition - transform.right * _enemyType.patrolRadius;
        _pointB = _originalPosition + transform.right * _enemyType.patrolRadius;
        _currentTarget = _pointA;
        this.gameObject.SetActive(true);
        _rb = this.GetComponent<Rigidbody2D>();
        _isInited = true;
        _currentPatroolPoint = false;
        _initialScaleX = this.transform.localScale.x;
    }
    void Update()
    {
        if (_isInited)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _playerController.transform.position);
            float distanceFromOrigin = Vector3.Distance(transform.position, _originalPosition);
            if (_chasing)
            {
                if (distanceFromOrigin > _enemyType.maxFollowDistance) //Stop following if too far from origin
                {
                    _chasing = false;
                    SetNewTarget(ClosestPatrolPoint());
                    _targetReachingTime = 0f;
                }
                else
                {
                    SetNewTarget( _playerController.transform.position);
                    _targetReachingTime = 0f;
                }
            }
            else
            {
                if (distanceToPlayer < _enemyType.playerSeekRadius)
                {
                    _chasing = true;
                }
                else
                {// Patrol logic
                    if (Mathf.Abs(transform.position.x- _currentTarget.x) < 0.3f || _targetReachingTime>= _targetReachingTimeLimit)
                    {
                        _currentPatroolPoint = !_currentPatroolPoint;
                        SetNewTarget((_currentPatroolPoint) ? _pointA : _pointB);
                        _targetReachingTime = 0f;                    
                    }
                }
            }
            Vector2 direction = (_currentTarget - transform.position);
            if (direction.magnitude > 0)
                direction = direction.normalized;
            Vector2 force = direction *( _enemyType.speed - _rb.velocity.magnitude);
            _rb.AddForce(force, ForceMode2D.Force);
            _targetReachingTime += Time.deltaTime;
        }
    }
    private void SetNewTarget(Vector2 newTarget)
    {
        _currentTarget = newTarget;
        this.transform.localScale = new Vector3(_initialScaleX * (Mathf.Sign(_currentTarget.x - transform.position.x) > 0 ? -1f : 1f), this.transform.localScale.y, this.transform.localScale.z);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            OnPlayerTouched?.Invoke();
        }
    }
    public void Despawn()
    {
        Destroy(this.gameObject);
    }
    private Vector3 ClosestPatrolPoint()
    {
        return (Vector3.Distance(transform.position, _pointA) < Vector3.Distance(transform.position, _pointB)) ? _pointA : _pointB;
    }
}
