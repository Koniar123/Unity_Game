using UnityEngine;

public class Ground : MonoBehaviour
{
    Player player;


    public float groundHeight;
    public float groundRight;
    public float screenRight;
    BoxCollider2D boxCollider;

    bool didGenerateGround = false;

    public Obstacle BoxTemplate;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();


        boxCollider = GetComponent<BoxCollider2D>();
        groundHeight = transform.position.y + (boxCollider.size.y / 2f);
        screenRight = Camera.main.transform.position.x * 2;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x -= player.velocity.x * Time.fixedDeltaTime;

        

        groundRight = transform.position.x + (boxCollider.size.x / 2f);

        if (groundRight < 0f)
        {
            Destroy(gameObject);
            return;
        }

        if (!didGenerateGround)
        {
            if (groundRight < screenRight)
            {
                didGenerateGround = true;
                generateGround();
            }
        }

        transform.position = pos;

    }

    void generateGround()
    {
        GameObject go = Instantiate(gameObject);
        BoxCollider2D goBoxCollider = go.GetComponent<BoxCollider2D>();
        Vector2 pos;

        float h1 = player.jumpVelocity * player.maxHoldJumpTime;
        float t = player.jumpVelocity / -player.gravity;
        float h2 = player.jumpVelocity * t + (0.5f * (player.gravity * (t * t)));
        float maxJumpHeight = Mathf.Max(h1, h2);
        float maxY = maxJumpHeight * 0.7f;
        maxY += groundHeight;
        float minY = 1;
        float actualyY = Random.Range(minY, maxY);
        


        pos.y = actualyY - goBoxCollider.size.y / 2f;
        if (pos.y > 2.7f)
        {
            pos.y = 2.7f;
        }

        float t1 = t + player.maxHoldJumpTime;
        float t2 = Mathf.Sqrt((2.0f * (maxY - actualyY)) / -player.gravity);
        float TotalTime = t1 + t2;
        float maxX = TotalTime * player.velocity.x;
        maxX *= 0.7f;
        maxX += groundRight;
        float minX = screenRight + 5f;
        float actualX = Random.Range(minX, maxX);


        pos.x = actualX + goBoxCollider.size.x / 2f;
        go.transform.position = pos;
        
        Ground goGround = go.GetComponent<Ground>();
        goGround.groundHeight = go.transform.position.y + (goBoxCollider.size.y / 2f);


        int obstacleNum = Random.Range(0, 3);
        for (int i = 0; i < obstacleNum; i++)
        {
            GameObject box = Instantiate(BoxTemplate.gameObject);
            float y = goGround.groundHeight;
            float halfWidth = goBoxCollider.size.x / 2f - 1f;
            float left = go.transform.position.x - halfWidth;
            float right = go.transform.position.x + halfWidth;
            float x = Random.Range(left, right);
            Vector2 boxPos = new Vector2(x, y);
            box.transform.position = boxPos;
        }
    }
}
