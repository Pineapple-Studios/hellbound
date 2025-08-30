using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

public class SetupInputActions : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite imageKeyboard;
    [SerializeField] private Sprite imageJoystick;

    private Image uiImage;
    private bool isUsingKeyboard;

    private void Awake()
    {
        uiImage = GetComponent<Image>();

        // Escuta quando o layout do controle ativo muda
        InputSystem.onAnyButtonPress.Call(OnAnyInput);
    }

    private void Start()
    {
        isUsingKeyboard = Keyboard.current != null && Keyboard.current.anyKey.isPressed;
        UpdateIcon();
    }

    private void OnAnyInput(InputControl control)
    {
        var device = control.device;

        bool isKeyboard = device is Keyboard || device is Mouse;
        if (isKeyboard != isUsingKeyboard)
        {
            isUsingKeyboard = isKeyboard;
            UpdateIcon();
        }
    }

    private void UpdateIcon()
    {
        if (uiImage == null) return;

        uiImage.sprite = isUsingKeyboard ? imageKeyboard : imageJoystick;
        uiImage.SetNativeSize();
    }
}
