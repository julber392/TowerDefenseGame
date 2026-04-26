using System;
using UnityEngine;

public class PlayerHp : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int _maxHealth = 100;

    [Header("Regeneration")]
    [SerializeField] private float _regenRate = 5f; 
    [SerializeField] private float _regenDelay = 2f; 

    private float _currentHealth;
    private bool _isDead;

    private float _lastDamageTime;

    public int CurrentHealth => Mathf.CeilToInt(_currentHealth);
    public int MaxHealth => _maxHealth;
    public bool IsDead => _isDead;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDiedPlayer;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, _maxHealth);
    }

    private void Update()
    {
        if (_isDead) return;

        HandleRegeneration();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }
    
    private void HandleRegeneration()
    {
        if (_currentHealth >= _maxHealth) return;
        
        if (Time.time < _lastDamageTime + _regenDelay) return;
        
        _currentHealth += _regenRate * Time.deltaTime;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        OnHealthChanged?.Invoke(CurrentHealth, _maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (_isDead) return;

        _lastDamageTime = Time.time;

        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        OnHealthChanged?.Invoke(CurrentHealth, _maxHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (_isDead) return;

        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        OnHealthChanged?.Invoke(CurrentHealth, _maxHealth);
    }

    private void Die()
    {
        if (_isDead) return;

        _isDead = true;
        OnDiedPlayer?.Invoke();
    }
}