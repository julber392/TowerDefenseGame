using System.Collections;
using UnityEngine;

public class ArrowVisual : MonoBehaviour, IAttackVisual
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float duration = 0.15f;

    private Coroutine arrowCoroutine;

    public void Play(EnemyEntity target)
    {
        if (arrowCoroutine != null)
            StopCoroutine(arrowCoroutine);

        arrowCoroutine = StartCoroutine(ShowArrow(target.transform));
    }

    private IEnumerator ShowArrow(Transform target)
    {
        if (target == null) yield break;

        Vector3 start = transform.position;
        Vector3 end = target.position;

        GameObject arrow = Instantiate(arrowPrefab, start, Quaternion.identity);

        Vector3 direction = (end - start).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle);

        float time = 0f;

        while (time < duration)
        {
            if (arrow == null) yield break;

            arrow.transform.position = Vector3.Lerp(start, end, time / duration);

            time += Time.deltaTime;
            yield return null;
        }
        
        arrow.transform.position = end;

        Destroy(arrow);
    }
}