using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //Variables 


    float horizontalMove;
    public float speed;

    Rigidbody2D myBody;

    bool grounded = false;

    Animator myAnim; 

    // these are the values that control the game feel of how the player moves. Tune these values to  match your wanted game feel
    public float castDist = 0.2f;
    public float gravityScale = 5f;
    public float gravityFall = 40f;
    public float jumpLimit = 2f;

    bool jump = false; 

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();  
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal"); //arrow keys for horizontal movement 
       // Debug.Log(horizontalMove); 

        //jumping 
        if (Input.GetButtonDown("Jump") && grounded) //only jump if we're on the ground 
        {
            jump = true; 
        }

        if (horizontalMove > 0.2f && horizontalMove < 0.2f)
        {
            myAnim.SetBool("Walking", true); //if we're moving walking aniamtion plays
        }else
        {
            myAnim.SetBool("Walking", false); //if we're not moving walking animation doesn't play
        }
    }


    private void FixedUpdate() //FixedUpdate runs consistent smoother physics 
    {
        float moveSpeed = horizontalMove * speed;

        if (jump) 
        {
            myBody.AddForce(Vector2.up * jumpLimit, ForceMode2D.Impulse); 
            jump = false;
        }

        //gravity settings 
        if(myBody.velocity.y > 0)
        {
            myBody.gravityScale = gravityScale;
        } else if(myBody.velocity.y < 0)
        {
            myBody.gravityScale = gravityFall; 
        }

        //raycast is a line emerging from our player that detects if we have hit something. Is like a collision but better 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDist); 
        Debug.DrawRay(transform.position, Vector2.down * castDist, Color.red); //this will show us the raycast

        if(hit.collider != null && hit.transform.name == "ground") //If I'm hitting the ground
        {
            grounded = true; 
        }else
        {
            grounded = false; 
        }


        myBody.velocity = new Vector3(moveSpeed, myBody.velocity.y, 0);


    }
}
