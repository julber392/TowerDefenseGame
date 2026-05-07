using UnityEngine;
using System.Collections.Generic;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private UpgradeCardUI cardPrefab;
    [SerializeField] private Transform container;

    [SerializeField] private TowerManager towerManager;
    [SerializeField] private PauseManager _pauseManager;

    private void Start()
    {
        GameEvents.OnLevelUp += Show;
    }

    private void OnDestroy()
    {
        GameEvents.OnLevelUp -= Show;
    }

    private void Show()
    {
        _pauseManager.Pause("levelup");
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
        _pauseManager.Resume("levelup");
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