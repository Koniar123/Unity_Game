using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;

    [Header("Camera")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 8f, -14f);
    [SerializeField] private float smoothness = 3f;

    private void LateUpdate()
    {
        float blend = 1f - Mathf.Exp(-smoothness * Time.deltaTime);

        transform.position = Vector3.Lerp(
            transform.position,
            target.position + offset,
            blend);

        transform.LookAt(target.position + Vector3.up * 2f);
    }
}