using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class ControlDisplayData
{
    public InputActionReference InputAction;
    public GameObject KeyboardHighlight;
    public Image GamepadImage;
    public string AnimatorTrigger;
    public string ActionName;
}

public class InputDisplayCtrl : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private string actionMapName = "Player";

    [Header("Controles")]
    [SerializeField] private List<ControlDisplayData> ControlDataList;

    [Header("UI")]
    [SerializeField] private TMP_Text ActionText;

    [Header("Animação")]
    [SerializeField] private Animator CharacterAnimator;

    [Header("Joystick")]
    [SerializeField] private RectTransform leftStickVisual;
    [SerializeField] private float stickMoveRadius = 50f;

    [Header("Cores")]
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color activeColor = Color.yellow;

    private InputActionMap activeMap;

    private void Start()
    {
        // Ativa o Action Map selecionado
        if (inputActions != null)
        {
            activeMap = inputActions.FindActionMap(actionMapName);
            if (activeMap != null)
            {
                activeMap.Enable();
                Debug.Log($"Action Map '{actionMapName}' ativado.");
            }
            else
            {
                Debug.LogWarning($"Action Map '{actionMapName}' não encontrado no InputActionAsset.");
            }
        }

        // Ativa individualmente as ações de ControlDataList (precaução)
        foreach (var data in ControlDataList)
        {
            if (data.InputAction != null && data.InputAction.action != null)
            {
                data.InputAction.action.Enable();
            }
        }
    }

    private void Update()
    {
        foreach (var data in ControlDataList)
        {
            if (data.InputAction == null || data.InputAction.action == null) continue;

            bool isPressed = data.InputAction.action.IsPressed();

            // DEBUG opcional
            // Debug.Log($"{data.ActionName}: {(isPressed ? "pressionado" : "não pressionado")}");

            // Ativa imagem azul sobre tecla
            if (data.KeyboardHighlight != null)
            {
                data.KeyboardHighlight.SetActive(isPressed);
            }

            // Muda cor do botão no joystick
            if (data.GamepadImage != null)
            {
                data.GamepadImage.color = isPressed ? activeColor : defaultColor;
            }

            // Trigger animação e texto
            if (isPressed)
            {
                if (CharacterAnimator && !string.IsNullOrEmpty(data.AnimatorTrigger))
                {
                    CharacterAnimator.SetTrigger(data.AnimatorTrigger);
                }
                else
                {
                    Debug.Log($"Animação '{data.AnimatorTrigger}' não executada (Animator não atribuído ou trigger vazio)");
                }

                if (ActionText)
                    ActionText.text = data.ActionName;
            }
        }

        UpdateStickVisual();
    }

    private void UpdateStickVisual()
    {
        if (Gamepad.current == null || leftStickVisual == null) return;

        Vector2 left = Gamepad.current.leftStick.ReadValue();
        leftStickVisual.anchoredPosition = left * stickMoveRadius;
    }
}