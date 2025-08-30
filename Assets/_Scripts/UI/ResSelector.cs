using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ResSelector : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI txtResolution;
    [SerializeField] private Button btnApply;
    [SerializeField] private GameObject btnVideo;

    [Header("Input")]
    [SerializeField] private InputActionAsset inputActions;

    private InputAction adjustAction;
    private InputAction submitAction;

    private List<Resolution> filteredRes = new();
    private int currentResolutionIndex = 0;
    private double currentRefreshRate;

    private bool inputInUse = false;

    private void Awake()
    {
        var uiMap = inputActions.FindActionMap("UI");
        adjustAction = uiMap.FindAction("Adjust");
        submitAction = uiMap.FindAction("Submit");

        adjustAction.performed += OnAdjust;
        submitAction.performed += OnSubmit;

        SetupResolutions();
        UpdateText();
    }

    private void OnEnable()
    {
        adjustAction.Enable();
        submitAction.Enable();
        btnApply.onClick.AddListener(ApplyResolution);
    }

    private void OnDisable()
    {
        adjustAction.Disable();
        submitAction.Disable();
        btnApply.onClick.RemoveListener(ApplyResolution);
    }

    private void SetupResolutions()
    {
        var allRes = Screen.resolutions;
        Vector2[] supported = {
            new(3840, 2160),
            new(2560, 1440),
            new(1920, 1080),
            new(1600, 900),
            new(1366, 768),
            new(1280, 720)
        };

        currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;
        filteredRes.Clear();

        foreach (var res in allRes)
        {
            foreach (var target in supported)
            {
                if (res.width == (int)target.x &&
                    res.height == (int)target.y &&
                    Mathf.Approximately((float)res.refreshRateRatio.value, (float)currentRefreshRate))
                {
                    filteredRes.Add(res);
                }
            }
        }

        for (int i = 0; i < filteredRes.Count; i++)
        {
            if (filteredRes[i].width == Screen.width &&
                filteredRes[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
    }

    private void OnAdjust(InputAction.CallbackContext context)
    {
        float horizontal = context.ReadValue<Vector2>().x;

        if (!inputInUse)
        {
            if (horizontal > 0.5f) NextResolution();
            else if (horizontal < -0.5f) PreviousResolution();

            inputInUse = true;
        }
    }

    private void LateUpdate()
    {
        if (Mathf.Abs(adjustAction.ReadValue<Vector2>().x) < 0.1f)
            inputInUse = false;
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
        ApplyResolution();
    }

    private void NextResolution()
    {
        currentResolutionIndex = (currentResolutionIndex + 1) % filteredRes.Count;
        UpdateText();
    }

    private void PreviousResolution()
    {
        currentResolutionIndex = (currentResolutionIndex - 1 + filteredRes.Count) % filteredRes.Count;
        UpdateText();
    }

    private void UpdateText()
    {
        var res = filteredRes[currentResolutionIndex];
        txtResolution.text = $"{res.width} X {res.height} - {res.refreshRateRatio.value:00} Hz";
        EventSystem.current.SetSelectedGameObject(txtResolution.gameObject);
    }

    private void ApplyResolution()
    {
        var res = filteredRes[currentResolutionIndex];
        Screen.SetResolution(res.width, res.height, FullScreenMode.FullScreenWindow);
        Debug.Log($"Resolução aplicada: {res.width}x{res.height} @ {res.refreshRateRatio.value}Hz");
        EventSystem.current.SetSelectedGameObject(btnVideo);
    }
}
