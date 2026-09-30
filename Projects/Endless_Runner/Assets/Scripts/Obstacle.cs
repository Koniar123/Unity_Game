using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Player player;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }   
    void Start()
    {
    Vector3 pos = transform.position;
    pos.y += 1.25f;
    transform.position = pos;
    }

    void Update()
    {
        
    }


    private void FixedUpdate()
    {
    Vector2 pos = transform.position;
    pos.x -= player.velocity.x * Time.fixedDeltaTime;

    if (pos.x < -25f)
    {
        Destroy(gameObject);
        return;
    }

    transform.position = pos;
    }

}  

