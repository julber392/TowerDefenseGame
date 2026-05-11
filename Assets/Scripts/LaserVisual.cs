using System.Collections;
using UnityEngine;

public class LaserVisual : MonoBehaviour, IAttackVisual
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float duration = 0.1f;

    private Coroutine laserCoroutine;

    private void Awake()
    {
        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }

    public void Play(EnemyEntity target)
    {
        if (lineRenderer == null) return;

        if (laserCoroutine != null)
            StopCoroutine(laserCoroutine);

        laserCoroutine = StartCoroutine(ShowLaser(target.transform));
    }

    private IEnumerator ShowLaser(Transform target)
    {
        Vector3 start = transform.position + new Vector3(0f, 0.7f, 0f);
        Vector3 end = target != null ? target.position : start;

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        yield return new WaitForSeconds(duration);

        lineRenderer.enabled = false;
    }
}