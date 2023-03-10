using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
using MouseButton = UnityEngine.InputSystem.LowLevel.MouseButton;

public class GamepadMouse : MonoBehaviour
{
    private Mouse _mouse;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private RectTransform cursorTransform;
    [SerializeField] private float sensi = 1000f;
    [SerializeField] private RectTransform canvasTransform;
    [SerializeField] private float padding = 15f;

    [SerializeField] private GameObject cursor;

    private bool _previousState;
    private const string gamepadScheme = "Gamepad";
    private const string mouseScheme = "Keyboard&Mouse";

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
           // _playerInput = new PlayerInput();
        }
        else
        {
           // _playerInput = GameObject.Find("Player").GetComponent<PlayerMovement>().inputActions;
        }
    }

    private void OnEnable()
    {
        if (_mouse == null)
        {
            _mouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }
        else if (!_mouse.added)
        {
            InputSystem.AddDevice(_mouse);
        }

        InputUser.PerformPairingWithDevice(_mouse);

        if (cursorTransform != null)
        {
            Vector2 pos = cursorTransform.anchoredPosition;
            InputState.Change(_mouse.position, pos);
        }
        InputSystem.onAfterUpdate += UpdateMotion;
        //InputSystem.onDeviceChange += OnDeviceChanged;
        //InputSystem.onDeviceChange += OnDeviceChanged;
        InputUser.onChange += OnDeviceChanged;
    }

    private void OnDisable()
    {
        InputSystem.onAfterUpdate -= UpdateMotion;
        InputUser.onChange -= OnDeviceChanged;
        InputSystem.RemoveDevice(_mouse);
    }

    private void UpdateMotion()
    {
        if (_mouse == null || Gamepad.current == null)
        {
            return;
        }

        Vector2 deltaValue = Gamepad.current.leftStick.ReadValue();
        deltaValue *= sensi * Time.unscaledDeltaTime;

        Vector2 curPos = _mouse.position.ReadValue();
        Vector2 newPos = curPos + deltaValue;

        newPos.x = Mathf.Clamp(newPos.x, 0, Screen.width - padding);
        newPos.y = Mathf.Clamp(newPos.y, 0, Screen.height - padding);

        InputState.Change(_mouse.position, newPos);
        InputState.Change(_mouse.delta, deltaValue);

        bool aButtonPressed = Gamepad.current.aButton.IsPressed();
        if (_previousState != aButtonPressed)
        {
            _mouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonPressed);
            InputState.Change(_mouse, mouseState);
            _previousState = aButtonPressed;
        }

        AnchorCursor(newPos);
    }

    private void AnchorCursor(Vector2 pos)
    {
        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, pos, null, out anchoredPos);
        cursorTransform.anchoredPosition = anchoredPos;
    }

    private void OnDeviceChanged(InputUser user, InputUserChange change, InputDevice device)
    {
        if(device != null)
        {
            bool found = false;
            foreach (var alias in device.allControls)
            {
                if (alias.ToString().Contains("Stick"))
                {
                    found = true;
                }
            }

            if (found)
            {
                cursor.SetActive(true);
                Cursor.visible = false;
            } else
            {
                cursor.SetActive(false);
                Cursor.visible = SceneManager.GetActiveScene().buildIndex == 0 || cursor.GetComponentInParent<Transform>().GetComponentInParent<PauseMenu>().active;
            }
        }
    }
}    
