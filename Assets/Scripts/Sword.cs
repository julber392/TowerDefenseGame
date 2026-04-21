using System;
using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _attackDuration = 0.8f;
    
    public event EventHandler OnSwordAttacked;
    private PolygonCollider2D _polygonCollider2D;
    
    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        AttackColliderOff();
    }

    public void Attack()
    {
        StartCoroutine(AttackRoutine());
        OnSwordAttacked?.Invoke(this, EventArgs.Empty);
    }
    
    private IEnumerator AttackRoutine()
    {
        AttackColliderOn();
        yield return new WaitForSeconds(_attackDuration);
        AttackColliderOff();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity))
        {
            enemyEntity.TakeDamage(_damage);
        }
    }

    private void AttackColliderOn()
    {
        _polygonCollider2D.enabled = true;
    }
    
    public void AttackColliderOff()
    {
        _polygonCollider2D.enabled = false;
    }
}