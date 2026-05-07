using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Follow")]
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private Vector3 offset = new(0, 0, -10);

    [Header("Bounds")]
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    private Vector3 velocity;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        float minX = minBounds.x + halfWidth;
        float maxX = maxBounds.x - halfWidth;

        float minY = minBounds.y + halfHeight;
        float maxY = maxBounds.y - halfHeight;
        if (minX > maxX)
        {
            float middle = (minBounds.x + maxBounds.x) * 0.5f;
            minX = middle;
            maxX = middle;
        }

        if (minY > maxY)
        {
            float middle = (minBounds.y + maxBounds.y) * 0.5f;
            minY = middle;
            maxY = middle;
        }

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

        targetPosition.z = -10f;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 center = new(
            (minBounds.x + maxBounds.x) / 2f,
            (minBounds.y + maxBounds.y) / 2f,
            0
        );

        Vector3 size = new(
            maxBounds.x - minBounds.x,
            maxBounds.y - minBounds.y,
            0
        );

        Gizmos.DrawWireCube(center, size);
    }
}