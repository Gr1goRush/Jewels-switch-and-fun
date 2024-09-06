using UnityEngine;
using TMPro;

public class VolumeController : MonoBehaviour
{ 
    [Header("ProgressText")]
    public TMP_Text musicText;
    public TMP_Text soundText;

    public UnityEngine.UI.Slider musicSlider, sfxSlider;

    public void Start()
    {
        float MusicValue = PlayerPrefs.GetFloat($"_MusicValue", 0f);
        float SFXValue = PlayerPrefs.GetFloat($"_SFXValue", 0f);

        AudioManager.Instance.MusicVolume(MusicValue);
        musicText.text = $"Music: " + (MusicValue * 100).ToString("F0") + "%";
        musicSlider.value = MusicValue;

        AudioManager.Instance.SFXVolume(SFXValue);
        soundText.text = $"SFX: " + (SFXValue * 100).ToString("F0") + "%";
        sfxSlider.value = SFXValue;
    }

    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(musicSlider.value);
        musicText.text = $"Music: " + (musicSlider.value * 100).ToString("F0") + "%";

    }
    public void SFXVolume() 
    {
        AudioManager.Instance.SFXVolume(sfxSlider.value);
        soundText.text = $"SFX: " + (sfxSlider.value * 100).ToString("F0") + "%";
    } 
}
