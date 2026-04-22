using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    [SerializeField] private PlayerHp playerHP;
    [SerializeField] private Slider slider;

    [Header("Smoothing")]
    [SerializeField] private float smoothSpeed = 10f;

    private float targetValue;

    private void Awake()
    {
        if (playerHP == null)
            playerHP = FindObjectOfType<PlayerHp>();

        if (slider == null)
            slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        playerHP.OnHealthChanged += UpdateBar;
    }

    private void OnDisable()
    {
        playerHP.OnHealthChanged -= UpdateBar;
    }

    private void Start()
    {
        float startValue = (float)playerHP.CurrentHealth / playerHP.MaxHealth;

        slider.value = startValue;
        targetValue = startValue;
    }

    private void Update()
    {
        slider.value = Mathf.Lerp(slider.value, targetValue, Time.deltaTime * smoothSpeed);
    }

    private void UpdateBar(int current, int max)
    {
        targetValue = (float)current / max;
    }
}