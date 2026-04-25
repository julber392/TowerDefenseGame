using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UpgradeCardUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button button;

    private TowerData tower;
    private UpgradeType type;
    private Action<TowerData, UpgradeType> callback;

    public void Setup(TowerData t, UpgradeType tp, Action<TowerData, UpgradeType> cb)
    {
        tower = t;
        type = tp;
        callback = cb;

        icon.sprite = tower.icon;

        text.text = GetText();

        button.onClick.AddListener(OnClick);
    }

    private string GetText()
    {
        switch (type)
        {
            case UpgradeType.Damage:
                return tower.towerName + " + урон";

            case UpgradeType.AttackSpeed:
                return tower.towerName + " быстрее";

            case UpgradeType.AddTower:
                return "+1 " + tower.towerName;

            default:
                return "";
        }
    }

    private void OnClick()
    {
        callback?.Invoke(tower, type);
    }
}