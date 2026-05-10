using System;
using System.Collections;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private DamageDealer damageDealer;
    [SerializeField] private float attackCooldown = 0.8f;

    public event EventHandler OnSwordAttacked;

    private PolygonCollider2D polygonCollider2D;

    private bool isAttacking;

    private void Awake()
    {
        polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        AttackColliderOff();
    }

    public void Attack()
    {
        if (isAttacking)
            return;

        isAttacking = true;

        AttackColliderOn();

        OnSwordAttacked?.Invoke(this, EventArgs.Empty);

        StartCoroutine(AttackCooldownRoutine());
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        damageDealer.DealDamage(collision.gameObject);
    }

    private void AttackColliderOn()
    {
        polygonCollider2D.enabled = true;
    }

    public void AttackColliderOff()
    {
        polygonCollider2D.enabled = false;
    }
}