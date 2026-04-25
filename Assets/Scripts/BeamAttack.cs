using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeamAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float range = 5f;
    [SerializeField] private TowerData data;
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
            timer = data.attackSpeed;
        }
    }

    private void Attack()
    {
        if (data == null) return;

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

        if (target == null) return;

        target.TakeDamage((int)data.damage);

        if (lineRenderer != null)
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
    public void SetData(TowerData towerData)
    {
        data = towerData;
    }
}