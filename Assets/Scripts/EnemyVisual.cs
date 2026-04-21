using System.Collections;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    private Animator animator;
    private EnemyEntity enemy;

    private const string IS_ATTACK = "IsAttack";
    private const string DEATH = "Death";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponentInParent<EnemyEntity>();
    }

    private void OnEnable()
    {
        enemy.OnDied += HandleDeath;
    }

    private void OnDisable()
    {
        enemy.OnDied -= HandleDeath;
    }

    private void Update()
    {
        animator.SetBool(IS_ATTACK, false);
    }

    public void SetAttackState(bool isAttacking)
    {
        animator.SetBool(IS_ATTACK, isAttacking);
    }

    private void HandleDeath()
    {
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        animator.SetTrigger(DEATH);
        
        yield return new WaitForSeconds(1.2f);

        GameEvents.OnEnemyKilled?.Invoke(enemy.XPReward);

        Destroy(gameObject);
    }
}