using UnityEngine;

public class BeamAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] public float range = 5f;
    [SerializeField] private TowerData data;
    [SerializeField] private LayerMask enemyLayer;

    private float timer;
    private IAttackVisual visual;

    private void Awake()
    {
        visual = GetComponent<IAttackVisual>();
    }
    private void Update()
    {
        if (data == null)
            return;

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

        visual?.Play(target);
    }

    public void SetData(TowerData towerData)
    {
        data = towerData;
    }
}