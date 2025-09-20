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
    [SerializeField] private GameObject pnlControl;

    [Space(10)]
    [Header("Buttons")]
    [SerializeField] private GameObject btnVideo;
    [SerializeField] private GameObject btnAudio;
    [SerializeField] private GameObject btnControl;

    [Space(10)]
    [Header("InputActions")]
    [SerializeField] private InputActionAsset inputActions;

    public static bool BlockTabSwitching = false;

    private InputAction switchTabAction;
    private GameObject lastSelected;

    private enum Tab { Video, Audio, Control }
    private static Tab lastOpenedTab;
    private Tab currentTab;

    // Confirmação dupla apenas para sair do painel de controls
    private bool awaitingConfirmation = false;
    private float confirmationTimeout = 2f;
    private float confirmationTimer;
    private int pendingTabIndex = -1;

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
        SelectCurrentTabButton();
    }

    private void Start()
    {
        StartCoroutine(ForceInitialButtonSelection());
    }

    private void OnDisable()
    {
        switchTabAction.Disable();
    }

    private void Update()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;

        if (current != null && current != lastSelected)
        {
            lastSelected = current;

            if (current == btnVideo) SwitchTo(Tab.Video);
            else if (current == btnAudio) SwitchTo(Tab.Audio);
            else if (current == btnControl) SwitchTo(Tab.Control);
        }

        // Confirmação de troca com timeout
        if (awaitingConfirmation)
        {
            confirmationTimer -= Time.unscaledDeltaTime;
            if (confirmationTimer <= 0f)
            {
                awaitingConfirmation = false;
                pendingTabIndex = -1;
                Debug.Log("Confirmação expirada.");
            }
        }
    }

    private IEnumerator ForceInitialButtonSelection()
    {
        yield return null;
        SelectCurrentTabButton();
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
        SelectCurrentTabButton();
    }

    private void OnSwitchTab(InputAction.CallbackContext context)
    {
        if (BlockTabSwitching) return;

        float dir = context.ReadValue<float>();
        if (Mathf.Abs(dir) < 0.5f) return;

        int direction = dir > 0 ? 1 : -1;
        int tabCount = System.Enum.GetNames(typeof(Tab)).Length;
        int nextTab = ((int)currentTab + direction + tabCount) % tabCount;

        // Verifica se estamos no painel de Controls e tentando sair
        if (currentTab == Tab.Control && (Tab)nextTab != Tab.Control)
        {
            if (awaitingConfirmation && nextTab == pendingTabIndex)
            {
                // Confirma a troca
                awaitingConfirmation = false;
                pendingTabIndex = -1;
                confirmationTimer = 0f;
                SwitchTo((Tab)nextTab);
            }
            else
            {
                // Arma a troca e aguarda nova tecla
                awaitingConfirmation = true;
                pendingTabIndex = nextTab;
                confirmationTimer = confirmationTimeout;
                Debug.Log($"Pressione novamente para trocar para '{(Tab)nextTab}'");
            }
            return;
        }

        // Para qualquer outro painel, troca na hora
        SwitchTo((Tab)nextTab);
    }

    private void SwitchTo(Tab targetTab)
    {
        if (targetTab == currentTab) return;

        currentTab = targetTab;
        lastOpenedTab = currentTab;

        awaitingConfirmation = false;
        pendingTabIndex = -1;
        confirmationTimer = 0f;

        UpdateTabVisual();
    }

    private void UpdateTabVisual()
    {
        pnlVideo.SetActive(currentTab == Tab.Video);
        pnlAudio.SetActive(currentTab == Tab.Audio);
        pnlControl.SetActive(currentTab == Tab.Control);

        SelectCurrentTabButton();
    }

    private void SelectCurrentTabButton()
    {
        if (currentTab == Tab.Video && btnVideo != null)
        {
            EventSystem.current.SetSelectedGameObject(btnVideo);
            pnlVideo?.SetActive(true);
        }
        else if (currentTab == Tab.Audio && btnAudio != null)
        {
            EventSystem.current.SetSelectedGameObject(btnAudio);
            pnlAudio?.SetActive(true);
        }
        else if (currentTab == Tab.Control && btnControl != null)
        {
            EventSystem.current.SetSelectedGameObject(btnControl);
            pnlControl?.SetActive(true);
        }
    }

    public void HideAllTabs()
    {
        pnlAudio.SetActive(false);
        pnlVideo.SetActive(false);
        pnlControl.SetActive(false);
    }

    public void ShowLastOpenedPanel()
    {
        SwitchTo(lastOpenedTab);
    }
}
