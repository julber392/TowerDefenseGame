using UnityEngine;
using System.Collections.Generic;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private UpgradeCardUI cardPrefab;
    [SerializeField] private Transform container;

    [SerializeField] private TowerManager towerManager;

    private void OnEnable()
    {
        GameEvents.OnLevelUp += Show;
    }

    private void OnDisable()
    {
        GameEvents.OnLevelUp -= Show;
    }

    private void Show()
    {
        Time.timeScale = 0f;
        panel.SetActive(true);

        GenerateCards();
    }

    private void GenerateCards()
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);

        var towers = towerManager.GetTowers();

        for (int i = 0; i < 3; i++)
        {
            var tower = towers[Random.Range(0, towers.Count)];
            var type = (UpgradeType)Random.Range(0, 3);

            var card = Instantiate(cardPrefab, container);
            card.Setup(tower, type, OnSelected);
        }
    }

    private void OnSelected(TowerData tower, UpgradeType type)
    {
        ApplyUpgrade(tower, type);

        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void ApplyUpgrade(TowerData tower, UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.Damage:
                tower.damage += 5;
                break;

            case UpgradeType.AttackSpeed:
                tower.attackSpeed *= 0.8f;
                break;

            case UpgradeType.AddTower:
                tower.count++;
                break;
        }
    }
}