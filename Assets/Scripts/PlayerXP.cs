using UnityEngine;

public class PlayerXP : MonoBehaviour
{
    [SerializeField] private int level = 1;

    [SerializeField] private float currentXP = 0f;
    [SerializeField] private float xpToNextLevel = 100f;
    [SerializeField] private float xpMultiplier = 1.5f;

    [SerializeField] private XPBarUI xpBar;

    private void OnEnable()
    {
        GameEvents.OnEnemyKilled += AddXP;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyKilled -= AddXP;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddXP(int amount)
    {
        currentXP += amount;

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }

        UpdateUI();
    }

    private void LevelUp()
    {
        level++;
        currentXP -= xpToNextLevel;
        xpToNextLevel *= xpMultiplier;

        Debug.Log("LEVEL UP! Уровень: " + level);
    }

    private void UpdateUI()
    {
        xpBar.SetXP(currentXP, xpToNextLevel, level);
        
    }
}