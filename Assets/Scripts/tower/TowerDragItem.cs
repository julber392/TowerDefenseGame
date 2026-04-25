using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerDragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TowerData towerData;
    private Canvas canvas;

    private GameObject dragGhost;
    private TowerPlacement placement;

    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("TowerUI").GetComponent<Canvas>();
    }

    private void Start()
    {
        placement = FindObjectOfType<TowerPlacement>();
        
    }

    public void Init(TowerData data)
    {
        towerData = data;
        icon.sprite = data.icon;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (towerData == null || towerData.count <= 0) return;

        dragGhost = new GameObject("Ghost", typeof(RectTransform), typeof(Image));
        
        var rect = dragGhost.GetComponent<RectTransform>();
        var img = dragGhost.GetComponent<Image>();

        img.sprite = towerData.icon;
        img.raycastTarget = false;
        img.color = Color.white;

        rect.sizeDelta = new Vector2(80, 80);

        dragGhost.transform.SetParent(canvas.transform, false);
        dragGhost.transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragGhost == null) return;

        dragGhost.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragGhost == null) return;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0;

        bool placed = placement.TryPlaceFromWorld(worldPos, towerData);

        if (placed)
        {
            towerData.count--;
        }

        Destroy(dragGhost);
    }
}