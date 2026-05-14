using UnityEngine;

public class TowerInventoryUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private TowerDragItem itemPrefab;
    [SerializeField] private TowerManager towerManager;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        foreach (var tower in towerManager.GetTowers())
        {
            var item = Instantiate(itemPrefab, container);

            item.Init(tower);

            var countUI = item.GetComponent<TowerCountUI>();

            if (countUI != null)
            {
                countUI.Init(tower, towerManager);
            }
        }
    }
}