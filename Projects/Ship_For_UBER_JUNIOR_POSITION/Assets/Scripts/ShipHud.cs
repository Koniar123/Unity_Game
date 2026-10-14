using UnityEngine;
using UnityEngine.UIElements;

[DefaultExecutionOrder(20)]
public class ShipHud : MonoBehaviour
{
    private const string OpeningLabelName = "sail-opening-label";
    private const string RotationLabelName = "sail-rotation-label";

    [SerializeField] private UIDocument _document;
    [SerializeField] private ShipController _shipController;
    [SerializeField] private ShipMovement _shipMovement;

    private Label _openingLabel;
    private Label _rotationLabel;

    private int _shownOpening;
    private int _shownRotation;

    private void OnEnable()
    {
        VisualElement root = _document.rootVisualElement;
        _openingLabel = root.Q<Label>(OpeningLabelName);
        _rotationLabel = root.Q<Label>(RotationLabelName);

        _shownOpening = int.MinValue;
        _shownRotation = int.MinValue;
    }

    private void Update()
    {
        int opening = Mathf.RoundToInt(_shipController.SailOpening);
        int rotation = Mathf.RoundToInt(_shipController.SailRotation);

        if (opening != _shownOpening)
        {
            _shownOpening = opening;
            _openingLabel.text = $"Sail opening: {opening}%";
        }

        if (rotation != _shownRotation)
        {
            _shownRotation = rotation;
            _rotationLabel.text = $"Sail rotation: {rotation}\u00B0";
        }
    }
}