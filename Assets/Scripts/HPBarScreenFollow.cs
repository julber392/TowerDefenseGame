using UnityEngine;

public class HPBarScreenFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0);

    private Camera cam;
    private RectTransform rectTransform;

    private void Awake()
    {
        cam = Camera.main;
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (target == null || cam == null) return;

        Vector3 worldPos = target.position + offset;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);
        
        if (screenPos.z < 0)
        {
            rectTransform.gameObject.SetActive(false);
            return;
        }
        else
        {
            rectTransform.gameObject.SetActive(true);
        }

        rectTransform.position = screenPos;
    }
}