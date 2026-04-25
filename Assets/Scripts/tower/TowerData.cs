using UnityEngine;

[CreateAssetMenu(menuName = "Tower/TowerData")]
public class TowerData : ScriptableObject
{
    public string towerName;
    public Sprite icon;
    public GameObject prefab;

    [Header("Stats")]
    public float damage = 10;
    public float attackSpeed = 1f;

    [Header("Inventory")]
    public int count = 1;
}