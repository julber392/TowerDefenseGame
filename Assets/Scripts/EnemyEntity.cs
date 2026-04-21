using UnityEngine;

public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _xpReward = 50;

    private int _currentHealth;
    public bool _isDead;

    public int XPReward => _xpReward;

    public event System.Action OnDied;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (_isDead) return;

        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _isDead = true;
            OnDied?.Invoke();
        }
    }
}