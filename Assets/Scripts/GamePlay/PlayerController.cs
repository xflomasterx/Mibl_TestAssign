using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Rigidbody2D rb;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.3f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Shooting")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _maxChargeTime;
    [SerializeField] private int _clipSize;

    public int AmmoQuantity => ammoQuantity;
    public int ClipSize => _clipSize;

    [Inject] private IInputService _input;
    [Inject] private Bullet.Pool _bulletPool;

    private readonly List<Bullet> _activeBullets = new List<Bullet>();
    public event Action<int, int> OnAmmoChanged;
    private Animator _player_animator;
    private float moveInput;
    private float shootChargingTime = 0f;
    private bool isShootCharging = false;
    private bool jumpRequested;
    private bool isGrounded;
    private bool isShotReady = true;
    private bool isFacingRight = true;
    private int ammoQuantity;

    private void Awake()
    {
        _player_animator = this.GetComponent<Animator>();
    }
    void OnEnable()
    {
        isShotReady = true;
        ammoQuantity = _clipSize;
    }
    void Update()
    {
        moveInput = 0f;
        bool isMoving = _input.IsActionRequested(CharacterAction.MoveLeft) || _input.IsActionRequested(CharacterAction.MoveRight);
        _player_animator.SetBool("isMoving", isMoving);
        if (_input.IsActionRequested(CharacterAction.MoveLeft))
        {
            if (isFacingRight)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            isFacingRight = false;
            moveInput = -1f;
        }
        if (_input.IsActionRequested(CharacterAction.MoveRight))
        {
            if (!isFacingRight)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            isFacingRight = true;
            moveInput = 1f;
        }
        if (_input.IsActionRequested(CharacterAction.Jump) && isGrounded)
        {
            jumpRequested = true;
        }       
        if (ammoQuantity>0)
        {
            if (_input.IsActionRequested(CharacterAction.Shoot))
            {
                shootChargingTime += Time.deltaTime;
                if (!isShootCharging)
                    isShootCharging = true;
            }
            if (isShotReady && ((isShootCharging & !_input.IsActionRequested(CharacterAction.Shoot)) || shootChargingTime >= _maxChargeTime))
            {
                isShotReady = false;
                isShootCharging = false;
                _player_animator.SetTrigger("Shoot");
            }
        }
    }
    public void FireBullet()
    {
        Vector2 shootDir = new Vector2(isFacingRight ? 1f : -1f, 0f); 
        var bullet = _bulletPool.Spawn(shootDir, shootChargingTime);

        bullet.transform.position = _firePoint.position;
        _activeBullets.Add(bullet);
        isShotReady = true;
        shootChargingTime = 0f;
        ammoQuantity--;
        OnAmmoChanged?.Invoke(ammoQuantity,_clipSize);
    }
    public void DespawnAllBullets()
    {
        foreach (var bullet in _activeBullets)
        {
            if (bullet.gameObject.activeInHierarchy)
                _bulletPool.Despawn(bullet);
        }
        _activeBullets.Clear();
    }
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        if (jumpRequested)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpRequested = false;
        }
    }
}