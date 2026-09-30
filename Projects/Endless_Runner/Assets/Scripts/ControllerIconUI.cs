using UnityEngine;
using UnityEngine.UI;

public class ControllerIconUI : MonoBehaviour
{
    public Sprite keyboardSprite;
    public Sprite xboxSprite;
    public Sprite playstationSprite;
    public Sprite switchSprite;
    public Sprite touchSprite;

    private Image _image;
    private CrossplayInput.InputDevice _lastDevice;

    void Awake()
    {
        _image = GetComponent<Image>();

        _lastDevice = CrossplayInput.InputDevice.KeyboardMouse;
        UpdateIcon(_lastDevice);
    }

    void Update()
    {
        var device = CrossplayInput.GetCurrentDevice();

        if (device != _lastDevice)
        {
            _lastDevice = device;
            UpdateIcon(device);
        }
    }

    void UpdateIcon(CrossplayInput.InputDevice device)
    {
        switch (device)
        {
            case CrossplayInput.InputDevice.PlayStation:
                _image.sprite = playstationSprite;
                break;

            case CrossplayInput.InputDevice.Xbox:
                _image.sprite = xboxSprite;
                break;

            case CrossplayInput.InputDevice.Switch:
                _image.sprite = switchSprite;
                break;

            case CrossplayInput.InputDevice.Touch:
                _image.sprite = touchSprite;
                break;

            default:
                _image.sprite = keyboardSprite;
                break;
        }
    }
}