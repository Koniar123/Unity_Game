using UnityEngine;

public class Parallax : MonoBehaviour
{

    public float depth = 1f;

    Player player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        float realVelocity = player.velocity.x / depth;
        Vector2 pos = transform.position;

        pos.x -= realVelocity * Time.fixedDeltaTime;

        if (pos.x < -35f)
        {
            pos.x = 100f;
        }

        transform.position = pos;
    }
}
