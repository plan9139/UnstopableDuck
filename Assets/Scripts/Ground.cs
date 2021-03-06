using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    Player player;

    public float groundHeight;
    public float groundRight;
    public float screenRight;
    BoxCollider2D collider;

    bool didGenerateGround = false;

    public Obstacle woodTemplate;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        collider = GetComponent<BoxCollider2D>();
        groundHeight = transform.position.y + (collider.size.y / 1.2f);
        screenRight = Camera.main.transform.position.x * 1;
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x -= player.velocity.x * Time.fixedDeltaTime;
        
        groundRight = transform.position.x + (collider.size.x / 1.1f);

        if(groundRight < 0 )
        {
            Destroy(gameObject);
            return;
        }

        if(!didGenerateGround)
        {

        
        if(groundRight < screenRight)
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
        BoxCollider2D goCollider = go.GetComponent<BoxCollider2D>();
        Vector2 pos;

        float h1 = player.jumpVelocity * player.MaxHoldJumpTime;
        float t = player.jumpVelocity / -player.gravity;
        float h2 = player.jumpVelocity * t + (0.5f * (player.gravity*(t*t)));
        float maxJumpHeight = h1 + h2;
        float maxY = maxJumpHeight * 1.2f;
        maxY += groundHeight;
        float minY = 0.9f;
        float actualY = Random.Range(minY, maxY);

        pos.y = actualY - goCollider.size.y / 2;
        if(pos.y > 6)
            pos.y = 6;

        float t1 = t + player.maxMaxHoldJumpTime;
        float t2 = Mathf.Sqrt((2.0f * (maxY - actualY)) / -player.gravity);
        float totalTime = t1 + t2;
        float maxX = totalTime * player.velocity.x;
        maxX *= 0.3f;
        maxX += groundRight;
        float minX = screenRight + 5;
        float actualX = Random.Range(minX , maxX);

        pos.x = actualX + goCollider.size.x / 2;
        go.transform.position = pos;

        
        Ground goGround = go.GetComponent<Ground>();
        goGround.groundHeight = go.transform.position.y + (goCollider.size.y / 1.2f);

        int obstacleNum = Random.Range(0, 3);
        for(int i=0; i<obstacleNum; i++)
        {
            GameObject wood = Instantiate(woodTemplate.gameObject);
            float y = goGround.groundHeight + 0.3f ;
            float halfWidth = goCollider.size.x / 2 - 1;
            float left =  go.transform.position.x - halfWidth;
            float right = go.transform.position.x + halfWidth;
            float x = Random.Range(left, right);
            Vector2 woodPos = new Vector2(x,y);
            wood.transform.position = woodPos;
        }
    }

}
