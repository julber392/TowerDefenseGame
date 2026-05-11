using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Roaming,
        Chase,
        Attack
    }
    private float baseSpeed;

    [Header("Target Point")]
    [SerializeField] private string targetTag = "Target";

    [Header("Player")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float aggroRange = 1.5f;
    [SerializeField] private float stopAggroRange = 2f;
    [SerializeField] private float attackDuration = 0.8f;
    [SerializeField] private float attackRange = 0.2f;
    [SerializeField] private float lastAttackTime = 0;
    [SerializeField] float attackCooldown = 0.5f;
    private bool isAttacking;
    private NavMeshAgent navMeshAgent;
    private Transform targetPoint;
    private Transform player;
    private EnemyEntity enemy;
    private State state;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        baseSpeed = navMeshAgent.speed;
        enemy = GetComponent<EnemyEntity>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        state = State.Roaming;
    }

    private void Start()
    {
        GameObject target = GameObject.FindGameObjectWithTag(targetTag);
        if (target != null)
            targetPoint = target.transform;
        else
            Debug.LogError("Не найден Target");
        
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("Не найден Player");
    }

    private void Update()
    {
        if (enemy._isDead)
        {
            StopEverything();
            return;
        }
        navMeshAgent.speed = baseSpeed * enemy.speedMultiplier;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (state)
        {
            case State.Roaming:
                MoveToPoint();

                if (distanceToPlayer < aggroRange)
                {
                    state = State.Chase;
                }
                break;

            case State.Chase:
                ChasePlayer(distanceToPlayer);

                if (distanceToPlayer > stopAggroRange)
                {
                    state = State.Roaming;
                }
                break;
            case State.Attack:
                
                break;
        }
    }

    private void MoveToPoint()
    {
        Vector3 targetPosition = targetPoint.position;

        ChangeFacingDirection(transform.position, targetPosition);
        navMeshAgent.SetDestination(targetPosition);
    }

    private void ChasePlayer(float distanceToPlayer)
    {
        ChangeFacingDirection(transform.position, player.position);

        if (!isAttacking && distanceToPlayer <= attackRange)
        {
            
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                lastAttackTime = Time.time;
                StartAttack();
            }
        }
        else if (!isAttacking)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(player.position);
        }
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
            transform.rotation = Quaternion.Euler(0, -180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    private void StopEverything()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.ResetPath();
    }
    private void StartAttack()
    {
        StartCoroutine(AttackRoutine());
    }
    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        state = State.Attack;

        navMeshAgent.isStopped = true;
        navMeshAgent.ResetPath();
        
        GetComponentInChildren<EnemyVisual>().PlayAttack();
        
        yield return new WaitForSeconds(attackDuration * 0.6f);
        
        enemy.DealDamage();
        
        yield return new WaitForSeconds(attackDuration * 0.6f);

        isAttacking = false;
        state = State.Chase;
    }
}