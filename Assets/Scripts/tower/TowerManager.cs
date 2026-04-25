using UnityEngine;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private List<TowerData> towers;

    public List<TowerData> GetTowers() => towers;

    public void AddTower(TowerData tower)
    {
        tower.count++;
    }

    public void UpgradeDamage(TowerData tower, float amount)
    {
        tower.damage += amount;
    }

    public void UpgradeAttackSpeed(TowerData tower, float percent)
    {
        tower.attackSpeed *= percent;
    }
}