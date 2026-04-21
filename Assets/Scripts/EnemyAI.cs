using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Roaming,
        Chase
    }

    [Header("Target Point")]
    [SerializeField] private string targetTag = "Target";

    [Header("Player")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float aggroRange = 1.5f;
    [SerializeField] private float stopAggroRange = 2f;

    private NavMeshAgent navMeshAgent;
    private Transform targetPoint;
    private Transform player;

    private State state;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        state = State.Roaming;
    }

    private void Start()
    {
        // точка назначения
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
        //if (player == null || targetPoint == null) return;

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
                ChasePlayer();

                if (distanceToPlayer > stopAggroRange)
                {
                    state = State.Roaming;
                }
                break;
        }
    }

    private void MoveToPoint()
    {
        Vector3 targetPosition = targetPoint.position;

        ChangeFacingDirection(transform.position, targetPosition);
        navMeshAgent.SetDestination(targetPosition);
    }

    private void ChasePlayer()
    {
        Vector3 playerPosition = player.position;

        ChangeFacingDirection(transform.position, playerPosition);
        navMeshAgent.SetDestination(playerPosition);
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
            transform.rotation = Quaternion.Euler(0, -180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}