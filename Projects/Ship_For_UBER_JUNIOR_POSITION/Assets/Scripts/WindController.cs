using UnityEngine;

public class WindController : MonoBehaviour
{
    [SerializeField] private Vector3 windDirection = Vector3.forward;
    [SerializeField] private float windSpeed = 10f;

    public Vector3 WindVelocity
    {
        get
        {
            return windDirection.normalized * windSpeed;
        }
    }

    public Vector3 WindDirection => windDirection.normalized;
    public float WindSpeed => windSpeed;
}
