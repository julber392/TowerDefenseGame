using System.Collections;
using UnityEngine;

public class EnemyEntity : MonoBehaviour, IDamageable
{
    public float speedMultiplier = 1f;
    private Coroutine slowCoroutine;
    
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _xpReward = 50;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _attackRange = 1.0f;
    public event System.Action OnAttack;
    private Transform _player;
    private int _currentHealth;
    public bool _isDead;

    public int XPReward => _xpReward;

    public event System.Action OnDied;

    private void Start()
    {
        _player = Player.Instance.transform;
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (_isDead) return;

        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _isDead = true;

            GameEvents.OnEnemyKilled?.Invoke(_xpReward); 
            OnDied?.Invoke();
        }
    }
    public void DealDamage()
    {
        if (_isDead) return;

            if (_player.TryGetComponent(out PlayerHp playerHp))
            {
                OnAttack?.Invoke();
                playerHp.TakeDamage(_damage);
            }
        
    }
    
    public void ApplySlow(float multiplier, float duration)
    {
        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);

        slowCoroutine = StartCoroutine(SlowRoutine(multiplier, duration));
    }

    private IEnumerator SlowRoutine(float multiplier, float duration)
    {
        speedMultiplier = multiplier;

        yield return new WaitForSeconds(duration);

        speedMultiplier = 1f;

    }
}