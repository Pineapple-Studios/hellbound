using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem.Controls;
using System.Linq;

public class InputDisplayCtrl : MonoBehaviour
{
    [System.Serializable]
    public class KeyBinding
    {
        public string keyName;
        public Image keyImage;
    }

    [Header("Sprites de Detecção de Input")]
    [SerializeField] private Sprite imageKeyboard;
    [SerializeField] private Sprite imageJoystick;
    [SerializeField] private Image deviceDisplayImage;

    [Header("UI de Teclado")]
    [SerializeField] private List<KeyBinding> keyboardBindings = new();

    [Header("UI de Mouse")]
    public Image leftClickImage;
    public Image rightClickImage;
    public Image middleClickImage;

    [Header("UI de Joystick")]
    public Image buttonSouthImage;
    public Image buttonEastImage;
    public Image dpadUpImage, dpadDownImage, dpadLeftImage, dpadRightImage;
    public RectTransform leftStickVisual;
    public RectTransform rightStickVisual;

    [Header("Visual")]
    [SerializeField] private float moveRadius = 50f;
    public Color defaultColor = Color.white;
    public Color activeColor = Color.yellow;

    private Dictionary<KeyControl, Image> keyToImage = new();
    private bool isUsingKeyboard = true;

    private bool GamepadWasUsed()
    {
        var g = Gamepad.current;
        return g.buttonSouth.wasPressedThisFrame || g.buttonEast.wasPressedThisFrame ||
               g.dpad.up.wasPressedThisFrame || g.dpad.down.wasPressedThisFrame ||
               g.leftStick.ReadValue() != Vector2.zero || g.rightStick.ReadValue() != Vector2.zero;
    }

    private void Start()
    {
        // Mapear teclas
        foreach (var binding in keyboardBindings)
        {
            var control = Keyboard.current?.allControls
            .FirstOrDefault(c => c.name.ToLower() == binding.keyName.ToLower() && c is KeyControl) as KeyControl;

            if (control != null && binding.keyImage != null)
                keyToImage[control] = binding.keyImage;
        }

        UpdateDeviceSprite(isUsingKeyboard);
    }

    private void Update()
    {
        DetectInputDevice();

        UpdateKeyboardVisual();
        UpdateMouseVisual();
        UpdateGamepadVisual();
        UpdateSticks();
    }

    private void DetectInputDevice()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!isUsingKeyboard)
            {
                isUsingKeyboard = true;
                UpdateDeviceSprite(true);
            }
        }
        else if (Gamepad.current != null && GamepadWasUsed())
        {
            if (isUsingKeyboard)
            {
                isUsingKeyboard = false;
                UpdateDeviceSprite(false);
            }
        }
    }

    private void UpdateDeviceSprite(bool usingKeyboard)
    {
        if (deviceDisplayImage == null) return;
        deviceDisplayImage.sprite = usingKeyboard ? imageKeyboard : imageJoystick;
        deviceDisplayImage.SetNativeSize();
    }

    private void UpdateKeyboardVisual()
    {
        foreach (var pair in keyToImage)
        {
            pair.Value.color = pair.Key.isPressed ? activeColor : defaultColor;
        }
    }

    private void UpdateMouseVisual()
    {
        if (Mouse.current == null) return;

        if (leftClickImage)
            leftClickImage.color = Mouse.current.leftButton.isPressed ? activeColor : defaultColor;

        if (rightClickImage)
            rightClickImage.color = Mouse.current.rightButton.isPressed ? activeColor : defaultColor;

        if (middleClickImage)
            middleClickImage.color = Mouse.current.middleButton.isPressed ? activeColor : defaultColor;
    }

    private void UpdateGamepadVisual()
    {
        if (Gamepad.current == null) return;

        if (buttonSouthImage)
            buttonSouthImage.color = Gamepad.current.buttonSouth.isPressed ? activeColor : defaultColor;

        if (buttonEastImage)
            buttonEastImage.color = Gamepad.current.buttonEast.isPressed ? activeColor : defaultColor;

        if (dpadUpImage)
            dpadUpImage.color = Gamepad.current.dpad.up.isPressed ? activeColor : defaultColor;
        if (dpadDownImage)
            dpadDownImage.color = Gamepad.current.dpad.down.isPressed ? activeColor : defaultColor;
        if (dpadLeftImage)
            dpadLeftImage.color = Gamepad.current.dpad.left.isPressed ? activeColor : defaultColor;
        if (dpadRightImage)
            dpadRightImage.color = Gamepad.current.dpad.right.isPressed ? activeColor : defaultColor;
    }

    private void UpdateSticks()
    {
        if (Gamepad.current == null) return;

        Vector2 left = Gamepad.current.leftStick.ReadValue();
        Vector2 right = Gamepad.current.rightStick.ReadValue();

        if (leftStickVisual)
            leftStickVisual.anchoredPosition = left * moveRadius;

        if (rightStickVisual)
            rightStickVisual.anchoredPosition = right * moveRadius;
    }
}
