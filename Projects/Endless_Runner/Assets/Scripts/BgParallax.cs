using UnityEngine;

public class BgParallax : MonoBehaviour
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

        if (pos.x < -28.8f)
        {
            pos.x = 85.6f;
        }

        transform.position = pos;
    }
}
