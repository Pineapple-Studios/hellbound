using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;

public class OptionsSelector : MonoBehaviour
{
    public static OptionsSelector Instance;

    [Header("Options Panel")]
    [SerializeField] private GameObject pnlVideo;
    [SerializeField] private GameObject pnlAudio;

    [Space(10)]
    [Header("Buttons")]
    [SerializeField] private GameObject btnVideo;
    [SerializeField] private GameObject btnAudio;

    [Space(10)]
    [Header("InputActions")]
    [SerializeField] private InputActionAsset inputActions;

    private InputAction switchTabAction;

    private GameObject lastSelected;

    private enum Tab { Video, Audio }
    private static Tab lastOpenedTab = Tab.Audio;
    private Tab currentTab;

    private void Start()
    {
        StartCoroutine(ForceInitialButtonSelection());
    }


    private void Update()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;

        if (current == null || current == lastSelected)
            return;

        lastSelected = current;

        if (current == btnVideo)
        {
            SwitchTo(Tab.Video);
        }
        else if (current == btnAudio)
        {
            SwitchTo(Tab.Audio);
        }
    }


    private void Awake()
    {
        Instance = this;
        currentTab = lastOpenedTab;
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
    private IEnumerator ForceInitialButtonSelection()
    {
        yield return null;

        if (currentTab == Tab.Video && btnVideo != null)
            EventSystem.current.SetSelectedGameObject(btnVideo);
        else if (currentTab == Tab.Audio && btnAudio != null)
            EventSystem.current.SetSelectedGameObject(btnAudio);
    }

    public void ForceSelectLastButton()
    {
        StartCoroutine(SelectLastButtonDelayed());
    }

    private IEnumerator SelectLastButtonDelayed()
    {
        yield return null;

        EventSystem.current.SetSelectedGameObject(null);

        yield return null;

        if (lastOpenedTab == Tab.Audio && btnAudio != null)
            EventSystem.current.SetSelectedGameObject(btnAudio);
        else if (lastOpenedTab == Tab.Video && btnVideo != null)
            EventSystem.current.SetSelectedGameObject(btnVideo);
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
        lastOpenedTab = currentTab;

        UpdateTabVisual();
    }

    private void UpdateTabVisual()
    {
        bool isVideo = currentTab == Tab.Video;

        pnlVideo.SetActive(isVideo);
        pnlAudio.SetActive(!isVideo);

        if (isVideo && btnVideo != null)
            EventSystem.current.SetSelectedGameObject(btnVideo);
        else if (!isVideo && btnAudio != null)
            EventSystem.current.SetSelectedGameObject(btnAudio);
    }

    public void HideAllTabs()
    {
        pnlAudio.SetActive(false);
        pnlVideo.SetActive(false);
    }

    public void ShowLastOpenedPanel()
    {
        SwitchTo(lastOpenedTab);
    }
}
