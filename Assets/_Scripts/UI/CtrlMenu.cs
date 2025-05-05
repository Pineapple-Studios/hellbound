using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CtrlMenu : MonoBehaviour
{
    public static CtrlMenu Instance;

    [Header("Buttons")]
    [SerializeField] Button btnPlay;
    [SerializeField] Button btnOptions;
    [SerializeField] Button btnExtras;
    [SerializeField] Button btnQuit;
    [Space(10)]
    [Header("InputActions")]
    [SerializeField] InputActionAsset inputActions;

    private InputAction moveUIAction;
    private InputAction confirmAction;
    private InputAction backAction;

    private AnimationCtrlMenu _acm;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        _acm = FindFirstObjectByType<AnimationCtrlMenu>();
        
        var uiMap = inputActions.FindActionMap("UI");
        moveUIAction = uiMap.FindAction("Move");
        confirmAction = uiMap.FindAction("Confirm");
        backAction = uiMap.FindAction("Back");
    }

    private void OnEnable()
    {
        inputActions.FindActionMap("UI").Enable();
        backAction.performed += OnBack;

        BtnEnable();
    }

    private void OnDisable()
    {
        backAction.performed -= OnBack;
        inputActions.FindActionMap("UI").Disable();
        BtnDisable();
    }

    void OnPlay()
    {
        Debug.Log("Play");
    }

    void OnOptions()
    {
        Debug.Log("Options");
        _acm.GoTo(MenuState.Options);
    }

    void OnExtras()
    {
        Debug.Log("Extras");
        _acm.GoTo(MenuState.Extras);
    }

    void OnQuit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    private void OnBack(InputAction.CallbackContext context)
    {
        if (AnimationCtrlMenu.Instance != null)
        {
            Debug.Log("Back Menu");
            AnimationCtrlMenu.Instance.GoBack();
        }
        else
        {
            Debug.LogWarning("AnimationCtrlMenu.Instance é null!");
        }
    }

    void BtnEnable()
    {
        btnPlay?.onClick.AddListener(OnPlay);
        btnOptions?.onClick.AddListener(OnOptions);
        btnExtras?.onClick.AddListener(OnExtras);
        btnQuit?.onClick.AddListener(OnQuit);
    }
    void BtnDisable()
    {
        btnPlay?.onClick.RemoveListener(OnPlay);
        btnOptions?.onClick.RemoveListener(OnOptions);
        btnExtras?.onClick.RemoveListener(OnExtras);
        btnQuit?.onClick.RemoveListener(OnQuit);
    }
}
