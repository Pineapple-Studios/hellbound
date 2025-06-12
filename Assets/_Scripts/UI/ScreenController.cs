using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ScreenController : MonoBehaviour
{
    public enum ScreenMode { Timed, WaitForAnyKey }

    [Header("Mode")]
    [SerializeField] private ScreenMode mode = ScreenMode.Timed;

    [Header("Scene")]
    [SerializeField] private string nextSceneName;
    [SerializeField] private float timeToGo = 3f;

    [Header("Input")]
    [SerializeField] private InputActionAsset inputActions;
    private InputAction anyKeyAction;

    private float _timeCounter = 0f;
    private bool _hasTriggered = false;

    private void OnEnable()
    {
        if (mode == ScreenMode.WaitForAnyKey)
        {
            var uiMap = inputActions.FindActionMap("UI");
            anyKeyAction = uiMap.FindAction("AnyKey");
            anyKeyAction.Enable();
            anyKeyAction.performed += OnAnyKeyPressed;
        }
    }

    private void OnDisable()
    {
        if (mode == ScreenMode.WaitForAnyKey && anyKeyAction != null)
        {
            anyKeyAction.performed -= OnAnyKeyPressed;
            anyKeyAction.Disable();
        }
    }

    void Update()
    {
        if (_hasTriggered) return;

        if (mode == ScreenMode.Timed)
        {
            _timeCounter += Time.deltaTime;
            if (_timeCounter >= timeToGo)
                LoadNextScene();
        }
    }

    private void OnAnyKeyPressed(InputAction.CallbackContext context)
    {
        if (_hasTriggered) return;
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        _hasTriggered = true;
        SceneManager.LoadScene(nextSceneName);
    }
}
