using TMPro;
using UnityEngine;

public class TowerCountUI : MonoBehaviour
{
    [SerializeField] private TMP_Text countText;

    private TowerData towerData;
    private TowerManager towerManager;

    public void Init(TowerData data, TowerManager manager)
    {
        towerData = data;
        towerManager = manager;

        towerManager.OnTowerCountChanged += OnTowerChanged;

        UpdateUI();
    }

    private void OnDestroy()
    {
        if (towerManager != null)
        {
            towerManager.OnTowerCountChanged -= OnTowerChanged;
        }
    }

    private void OnTowerChanged(TowerData changedTower)
    {
        if (changedTower != towerData)
            return;

        UpdateUI();
    }

    private void UpdateUI()
    {
        countText.text = $"{towerData.count}";
    }
}