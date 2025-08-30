using FMODUnity;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonHoverSound : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button btn;
    [SerializeField] private TMP_Dropdown dpd;
    [SerializeField] private Slider sld;

    [Header("Play on Enable")]
    [SerializeField] private bool playOnEnable = false;

    private void OnEnable()
    {
        if (btn != null)
        {
            btn.onClick.AddListener(PlayClickSound);
            if (playOnEnable)
                PlayMoveSound();
        }

        if (dpd != null)
        {
            dpd.onValueChanged.AddListener(_ => PlayClickSound());
        }

        if (sld != null)
        {
            sld.onValueChanged.AddListener(_ => PlaySliderSound());
        }
    }

    private void OnDisable()
    {
        if (btn != null)
            btn.onClick.RemoveListener(PlayClickSound);

        if (dpd != null)
            dpd.onValueChanged.RemoveAllListeners();

        if (sld != null)
            sld.onValueChanged.RemoveAllListeners();
    }

    private void PlayClickSound()
    {
        if (FMODAudioManager.Instance && FMODEventsUI.Instance)
            FMODAudioManager.Instance.PlayOneShot(FMODEventsUI.Instance.clickUI, transform.position);
    }

    private void PlayMoveSound()
    {
        if (FMODAudioManager.Instance && FMODEventsUI.Instance)
            FMODAudioManager.Instance.PlayOneShot(FMODEventsUI.Instance.moveUI, transform.position);
    }

    private void PlaySliderSound()
    {
        if (FMODAudioManager.Instance && FMODEventsUI.Instance)
            FMODAudioManager.Instance.PlayOneShot(FMODEventsUI.Instance.sldMove, transform.position);
    }
}
