using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI levelText;

    public void SetXP(float current, float max, int level)
    {
        slider.maxValue = max;
        slider.value = current;
        
        levelText.text = "Lvl " + level;
        
    }
}