using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeamAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float range = 5f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Laser")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float laserDuration = 0.1f;

    [Header("Enemy")]
    [SerializeField] private LayerMask enemyLayer;

    private float timer;

    private void Awake()
    {
        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Attack();
            timer = attackCooldown;
        }
    }

    private void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);

        if (hits.Length == 0) return;

        EnemyEntity target = null;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out EnemyEntity enemy))
            {
                target = enemy;
                break;
            }
        }
        
        target.TakeDamage(damage);

        StartCoroutine(ShowLaser(target.transform.position));
    }

    private IEnumerator ShowLaser(Vector3 target)
    {
        
        lineRenderer.enabled = true;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, target);

        yield return new WaitForSeconds(laserDuration);

        lineRenderer.enabled = false;
    }
}