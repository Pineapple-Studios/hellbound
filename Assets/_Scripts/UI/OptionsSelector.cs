using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;

public class OptionsTabManager : MonoBehaviour
{
    [Header("Painéis de Opções")]
    [SerializeField] private GameObject panelVideo;
    [SerializeField] private GameObject panelAudio;

    [Header("Botões (opcional)")]
    [SerializeField] private GameObject btnVideo;
    [SerializeField] private GameObject btnAudio;

    [Header("Input")]
    [SerializeField] private InputActionAsset inputActions;

    private InputAction switchTabAction;

    private enum Tab { Video, Audio }
    private Tab currentTab = Tab.Audio;

    private void Start()
    {
        StartCoroutine(ForceInitialButtonSelection());
    }

    private IEnumerator ForceInitialButtonSelection()
    {
        yield return null;

        if (currentTab == Tab.Video && btnVideo != null)
            EventSystem.current.SetSelectedGameObject(btnVideo);
        else if (currentTab == Tab.Audio && btnAudio != null)
            EventSystem.current.SetSelectedGameObject(btnAudio);
    }

    private void Awake()
    {
        var uiMap = inputActions.FindActionMap("UI");
        switchTabAction = uiMap.FindAction("SwitchTab");
        switchTabAction.performed += OnSwitchTab;
    }

    private void OnEnable()
    {
        switchTabAction.Enable();
        UpdateTabVisual();
        if (currentTab == Tab.Video && btnVideo != null)
            EventSystem.current.SetSelectedGameObject(btnVideo);
        else if (currentTab == Tab.Audio && btnAudio != null)
            EventSystem.current.SetSelectedGameObject(btnAudio);
    }

    private void OnDisable()
    {
        switchTabAction.Disable();
    }

    private void OnSwitchTab(InputAction.CallbackContext context)
    {
        float dir = context.ReadValue<float>();

        if (dir > 0.5f)
            SwitchTo(Tab.Audio);
        else if (dir < -0.5f)
            SwitchTo(Tab.Video);
    }

    private void SwitchTo(Tab targetTab)
    {
        if (targetTab == currentTab) return;

        currentTab = targetTab;
        UpdateTabVisual();
    }

    private void UpdateTabVisual()
    {
        bool isVideo = currentTab == Tab.Video;

        panelVideo.SetActive(isVideo);
        panelAudio.SetActive(!isVideo);

        // Se quiser manter navegação UI fluida
        if (isVideo && btnVideo != null)
            EventSystem.current.SetSelectedGameObject(btnVideo);
        else if (!isVideo && btnAudio != null)
            EventSystem.current.SetSelectedGameObject(btnAudio);
    }
}
