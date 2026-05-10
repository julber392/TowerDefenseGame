using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] private Transform enemySpawn;
    [Header("Target Block")]
    [SerializeField] private Transform target;
    [SerializeField] private float targetBlockRadius = 1f;
    
    [Header("Grid Settings")]
    public int width = 10;
    public int height = 10;
    public float cellSize = 1f;

    [Header("Tower")]
    public GameObject towerPrefab;

    [Header("Grid Origin")]
    public Transform gridManager; 

    private bool[,] grid;

    void Start()
    {
        grid = new bool[width, height];
    }
    
    Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector3 origin = gridManager.position;

        int x = Mathf.FloorToInt((worldPos.x - origin.x) / cellSize);
        int y = Mathf.FloorToInt((worldPos.y - origin.y) / cellSize);

        return new Vector2Int(x, y);
    }

    bool CanPlace(int x, int y)
    {
        if (x < 0 || y < 0 || x + 1 >= width || y + 1 >= height)
            return false;

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (grid[x + i, y + j])
                    return false;
            }
        }

        return true;
    }

    void TryPlaceTower(int x, int y)
    {
        if (!CanPlace(x, y))
        {
            Debug.Log(" нельзя поставить");
            return;
        }

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                grid[x + i, y + j] = true;
            }
        }

        Vector3 origin = gridManager.position;

        Vector3 spawnPos = new Vector3(
            origin.x + (x + 1) * cellSize,
            origin.y + (y + 1) * cellSize,
            0
        );

        Instantiate(towerPrefab, spawnPos, Quaternion.identity);
    }

    void OnDrawGizmos()
    {
        if (gridManager == null) return;

        Gizmos.color = Color.gray;

        Vector3 origin = gridManager.position;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(
                    origin.x + x * cellSize,
                    origin.y + y * cellSize,
                    0
                );

                Gizmos.DrawWireCube(
                    pos + new Vector3(cellSize / 2, cellSize / 2, 0),
                    new Vector3(cellSize, cellSize, 0)
                );
            }
        }
    }
    public bool TryPlaceFromWorld(Vector3 worldPos, TowerData towerData)
    {
        Vector2Int gridPos = WorldToGrid(worldPos);
        
        if (IsTooCloseToTarget(worldPos))
        {
            Debug.Log("Слишком близко к target");
            return false;
        }
        
        if (!CanPlace(gridPos.x, gridPos.y))
            return false;

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                grid[gridPos.x + i, gridPos.y + j] = true;
            }
        }

        Vector3 origin = gridManager.position;

        Vector3 spawnPos = new Vector3(
            origin.x + (gridPos.x + 1) * cellSize,
            origin.y + (gridPos.y + 1) * cellSize,
            0
        );

        GameObject towerGO = Instantiate(towerData.prefab, spawnPos, Quaternion.identity);
        NavMeshObstacle obstacle = towerGO.GetComponent<NavMeshObstacle>();

        if (obstacle != null)
        {
            obstacle.carving = true;
        }

        StartCoroutine(CheckPathNextFrame(towerGO, gridPos, towerData));

        return true;
    }
    private IEnumerator CheckPathNextFrame(
        GameObject towerGO,
        Vector2Int gridPos,
        TowerData towerData
    )
    {
        yield return null;

        if (!HasPath())
        {
            Debug.Log("Нельзя полностью блокировать путь!");

            Destroy(towerGO);
            
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    grid[gridPos.x + i, gridPos.y + j] = false;
                }
            }

            yield break;
        }

        BeamAttack attack = towerGO.GetComponent<BeamAttack>();

        if (attack != null)
        {
            attack.SetData(towerData);
        }
    }
    bool IsTooCloseToTarget(Vector3 worldPos)
    {
        return Vector3.Distance(worldPos, target.position) < targetBlockRadius;
    }
    
    private bool HasPath()
    {
        NavMeshPath path = new NavMeshPath();

        bool pathFound = NavMesh.CalculatePath(
            enemySpawn.position,
            target.position,
            NavMesh.AllAreas,
            path
        );

        return pathFound && path.status == NavMeshPathStatus.PathComplete;
    }
    
}