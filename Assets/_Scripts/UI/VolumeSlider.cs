using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType { MASTER, MUSIC, SFX }

    [SerializeField] private VolumeType type;
    private Slider slider;

    private void Awake() => slider = GetComponentInChildren<Slider>();

    private void OnEnable()
    {
        var fmod = FMODAudioManager.Instance;
        switch (type)
        {
            case VolumeType.MASTER: slider.value = fmod.masterVolume; break;
            case VolumeType.MUSIC: slider.value = fmod.musicVolume; break;
            case VolumeType.SFX: slider.value = fmod.sfxVolume; break;
        }
    }

    public void OnSliderChanged()
    {
        var val = slider.value;
        var fmod = FMODAudioManager.Instance;

        switch (type)
        {
            case VolumeType.MASTER: fmod.masterVolume = val; break;
            case VolumeType.MUSIC: fmod.musicVolume = val; break;
            case VolumeType.SFX: fmod.sfxVolume = val; break;
        }

        // Salva no PlayerPrefs apenas quando mudar
        fmod.SaveVolumeSettings();

        // Toca o feedback de UI
        fmod.PlayOneShot(FMODEventsUI.Instance.sldMove, transform.position);
    }
}
