using System.Collections;
using UnityEngine;

public class SlowPulseVisual : MonoBehaviour, IAttackVisual
{
    [Header("Visual")]
    [SerializeField] private GameObject pulsePrefab;
    [SerializeField] private float duration = 0.8f;
    [SerializeField] private float maxScale = 8f;

    [Header("Slow")]
    [SerializeField] private float slowRadius = 8f;
    [SerializeField] private float slowMultiplier = 0.3f;
    [SerializeField] private float slowDuration = 3f;

    [SerializeField] private LayerMask enemyLayer;

    public void Play(EnemyEntity target)
    {
        ApplySlow();
        StartCoroutine(PulseRoutine());
    }

    private void ApplySlow()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            slowRadius,
            enemyLayer
        );

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out EnemyEntity enemy))
            {
                enemy.ApplySlow(slowMultiplier, slowDuration);
            }
        }
    }

    private IEnumerator PulseRoutine()
    {
        GameObject pulse = Instantiate(
            pulsePrefab,
            transform.position,
            Quaternion.identity
        );

        SpriteRenderer sr = pulse.GetComponent<SpriteRenderer>();

        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one * maxScale;

        Color startColor = sr.color;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / duration);

            pulse.transform.localScale =
                Vector3.Lerp(startScale, endScale, t);

            Color c = startColor;
            c.a = Mathf.Lerp(1f, 0f, t);

            sr.color = c;

            yield return null;
        }

        Destroy(pulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, slowRadius);
    }
}