using UnityEngine;

public class WindController : MonoBehaviour
{
[Header("Wind Settings")]
[SerializeField] private Vector3 windDirection = Vector3.forward;
[SerializeField] private float windSpeed = 10f;

public Vector3 WindVelocity => WindDirection * windSpeed;

public Vector3 WindDirection
{
    get
    {
        Vector3 direction = windDirection;
        direction.y = 0f;
        return direction.normalized;
    }
}

public float WindSpeed => windSpeed;

private void OnDrawGizmosSelected()
{
    Gizmos.color = Color.cyan;
    Gizmos.DrawRay(transform.position, WindDirection * 5f);
}

}