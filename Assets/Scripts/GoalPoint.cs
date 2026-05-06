using UnityEngine;

public class GoalPoint : MonoBehaviour
{
    [SerializeField] private int enemiesToLose = 5;
    [SerializeField] private GameOverUI gameOverUI;

    private int currentEnemies = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyEntity enemy))
        {
            currentEnemies++;
            
            Destroy(enemy.gameObject);

            if (currentEnemies >= enemiesToLose)
            {
                gameOverUI.ShowGameOver();
            }
        }
    }
}