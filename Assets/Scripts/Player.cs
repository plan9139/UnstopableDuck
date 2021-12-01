using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float gravity;
    public Vector2 velocity;
    public float maxAcceleration = 10;
    public float acceleration = 10;
    public float distance = 0;
    public float groundHeight = 10;
    public float jumpVelocity = 20;
    public float maxVelocity = 100;
    public bool isGrounded = false;
    public bool isDashing = false;
    public bool isHoldingJump = false;
    public float MaxHoldJumpTime = 0.4f;
    public float maxMaxHoldJumpTime = 0.4f;
    public float holdJumpTimer = 0.0f;

    public float jumpGroundThreshold = 1;

    public bool isDead = false;
    private Animator anim;
    private AudioSource au;
    public AudioClip dashClip;
    public AudioClip crackClip;

    
    void Start()
    {
        anim = GetComponent<Animator>();
        au = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        Vector2 pos =transform.position;
        float groundDistance = Mathf.Abs(pos.y - groundHeight);

        if(isGrounded || groundDistance <= jumpGroundThreshold)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                isGrounded = false;
                velocity.y = jumpVelocity;
                isHoldingJump = true;
                holdJumpTimer = 0; 
            }
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            isHoldingJump = false;
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            isDashing = true;
            anim.SetBool("isDash",true);
            au.clip = dashClip;
            au.Play();
            
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            isDashing = false;
            velocity.x *= 0.4f;
            anim.SetBool("isDash",false);
        }

        
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;

        float velocityRatio = velocity.x /maxVelocity;
            acceleration = maxAcceleration * (0.5f - velocityRatio);
            MaxHoldJumpTime = maxMaxHoldJumpTime * velocityRatio;

            if(isDashing){
                velocity.x += acceleration * 0.9f;
                
            }

            else{
            velocity.x += acceleration * Time.fixedDeltaTime;
            }
            
            if(velocity.x >= maxVelocity)
            {
                velocity.x = maxVelocity;
            }

        if(isDead)
        {
            return;
        }


        if(pos.y < -20 )
        {
            isDead = true;
        }

        if(!isGrounded)
        {
            if(isHoldingJump)
            {
                holdJumpTimer += Time.fixedDeltaTime;
                if(holdJumpTimer >= MaxHoldJumpTime)
                {
                    isHoldingJump = false;
                }
            
                
            }


            pos.y += velocity.y * Time.fixedDeltaTime;
            
            if(!isHoldingJump)
            {

            velocity.y += gravity * Time.fixedDeltaTime;
            
            }

            Vector2 rayOrigin = new Vector2(pos.x + 0.3f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            if(hit2D.collider != null)
            {
                Ground ground = hit2D.collider.GetComponent<Ground>();
                if(ground != null)
                {
                    groundHeight = ground.groundHeight;
                    pos.y = groundHeight;
                    isGrounded = true;
                }
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);

            Vector2 wallOrigin = new Vector2(pos.x, pos.y);
            Vector2 wallDir = Vector2.right;
            RaycastHit2D wallHit = Physics2D.Raycast(wallOrigin, wallDir, velocity.x * Time.fixedDeltaTime);
            if(wallHit.collider != null)
            {
                Ground ground = wallHit.collider.GetComponent<Ground>();
                if(ground != null)
                {
                    if(pos.y < ground.groundHeight)
                    {
                        velocity.x = 0;

                    }
                }
            }
           
        }

        distance += velocity.x * Time.fixedDeltaTime;


        if(isGrounded)
        {
            
            

            Vector2 rayOrigin = new Vector2(pos.x - 0.2f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            if(hit2D.collider == null)
            {
                isGrounded = false;
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.yellow);

        }

        Vector2 obstOrigin = new Vector2(pos.x, pos.y);
        RaycastHit2D obstHitx = Physics2D.Raycast(obstOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime);
        if(obstHitx.collider != null)
        {
            Obstacle obstacle = obstHitx.collider.GetComponent<Obstacle>();
            if(obstacle !=null)
            {
                hitObstacle(obstacle);
            }
        }

        RaycastHit2D obstHity = Physics2D.Raycast(obstOrigin, Vector2.up, velocity.y * Time.fixedDeltaTime);
        if(obstHity.collider != null)
        {
            Obstacle obstacle = obstHity.collider.GetComponent<Obstacle>();
            if(obstacle !=null)
            {
                hitObstacle(obstacle);
            }
        }



        transform.position = pos;
    }

    void hitObstacle(Obstacle obstacle)
    {
        Destroy(obstacle.gameObject);
        au.clip = crackClip;
        au.Play();
        velocity.x *= 0.8f;
    }

    
}
