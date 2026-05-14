using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private List<TowerData> towers;

    public Action<TowerData> OnTowerCountChanged;
    public Action<TowerData> OnTowerStatsChanged;

    public List<TowerData> GetTowers() => towers;
    

    public bool TryUseTower(TowerData tower)
    {
        if (tower.count <= 0)
            return false;

        tower.count--;

        OnTowerCountChanged?.Invoke(tower);

        return true;
    }

    public void AddTower(TowerData tower, int amount = 1)
    {
        tower.count += amount;

        OnTowerCountChanged?.Invoke(tower);
    }

    public void UpgradeDamage(TowerData tower, float amount)
    {
        tower.damage += amount;

        OnTowerStatsChanged?.Invoke(tower);
    }


    public void UpgradeAttackSpeed(TowerData tower, float multiplier)
    {
        tower.attackSpeed *= multiplier;

        OnTowerStatsChanged?.Invoke(tower);
    }
}