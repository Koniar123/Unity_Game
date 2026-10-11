using UnityEngine;
using UnityEngine.UIElements;

[DefaultExecutionOrder(20)]
public class ShipHud : MonoBehaviour
{
    private const string OpeningLabelName = "sail-opening-label";
    private const string RotationLabelName = "sail-rotation-label";

    [Header("References")]
    [SerializeField] private UIDocument document;
    [SerializeField] private ShipController shipController;

    private Label openingLabel;
    private Label rotationLabel;
    private int shownOpening = int.MinValue;
    private int shownRotation = int.MinValue;

    private void OnEnable()
    {
        VisualElement root = document.rootVisualElement;

        openingLabel = root.Q<Label>(OpeningLabelName);
        rotationLabel = root.Q<Label>(RotationLabelName);

        // Force a refresh on the next Update.
        shownOpening = int.MinValue;
        shownRotation = int.MinValue;
    }

    private void Update()
    {
        int opening = Mathf.RoundToInt(shipController.SailOpening);
        int rotation = Mathf.RoundToInt(shipController.SailRotation);

        if (opening != shownOpening)
        {
            shownOpening = opening;
            openingLabel.text = $"Sail opening: {opening}%";
        }

        if (rotation != shownRotation)
        {
            shownRotation = rotation;
            rotationLabel.text = $"Sail rotation: {rotation}\u00B0";
        }
    }
}