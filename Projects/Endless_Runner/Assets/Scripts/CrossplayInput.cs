using UnityEngine;
using UnityEngine.InputSystem;

public static class CrossplayInput
{
    public enum InputDevice
    {
        KeyboardMouse,
        Xbox,
        PlayStation,
        Switch,
        Touch
    }

    private static InputDevice _lastDevice = InputDevice.KeyboardMouse;


    public static bool JumpWasPressedThisFrame()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            _lastDevice = InputDevice.KeyboardMouse;
            return true;
        }

        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            _lastDevice = InputDevice.Xbox;
            return true;
        }

        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.wasPressedThisFrame &&
                touch.position.ReadValue().x > Screen.width * 0.5f)
            {
                _lastDevice = InputDevice.Touch;
                return true;
            }
        }

        return false;
    }

    public static bool JumpWasReleasedThisFrame()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            _lastDevice = InputDevice.KeyboardMouse;
            return true;
        }

        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasReleasedThisFrame)
        {
            _lastDevice = InputDevice.Xbox;
            return true;
        }

        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.wasReleasedThisFrame &&
                touch.position.ReadValue().x > Screen.width * 0.5f)
            {
                _lastDevice = InputDevice.Touch;
                return true;
            }
        }

        return false;
    }

    public static InputDevice GetCurrentDevice()
    {
        return _lastDevice;
    }
}