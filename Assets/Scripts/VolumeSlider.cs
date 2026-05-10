using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI volumeText;

    private void Start()
    {
        float volume = AudioManager.Instance.GetVolume();

        slider.value = volume;

        UpdateVolumeText(volume);

        slider.onValueChanged.AddListener(ChangeVolume);
    }

    private void ChangeVolume(float value)
    {
        AudioManager.Instance.SetVolume(value);

        UpdateVolumeText(value);
    }

    private void UpdateVolumeText(float value)
    {
        int percent = Mathf.RoundToInt(value * 100f);

        volumeText.text = "SOUND SETTINGS\n" + percent + "%";
    }
}