using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //VARIABLES 

    //Respawining
    private Vector3 startingPosition = new Vector3(-2, -3, 1);
    private Vector3 lastCheckpointPosition = new Vector3(17, 1, 1);
    private bool atCheckpoint;

    float horizontalMove;
    public float speed;

    //jumping mechanics
    bool doubleJump = false; //setting up double jump
    bool wallJump = false; // setting up walljump
  
   
   

   Rigidbody2D myBody;

    bool grounded = false;

    Animator myAnim; 

    // these are the values that control the game feel of how the player moves. Tune these values to  match your wanted game feel
    public float castDist = 0.2f;
    public float gravityScale = 5f;
    public float gravityFall = 0.5f;
    public float jumpLimit = 5f;

    bool jump = false; 

    // Start is called before the first frame update
    void Start()
    {
  
        myBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        atCheckpoint = false; //We haven't hit checkpoint
    }





    //Dying and Respawning 
    void OnCollisionEnter2D(Collision2D collision) //player colliding with stuff that can kill it
    {
        if (collision.gameObject.tag == "lava") //if player collides with lava
        {
            Respawn(); //when player dies has to decide where to respawn
        }

        if (collision.gameObject.tag == "enemy") //if player collides with enemy
        {
            Respawn(); //when player dies has to decide where to respawn
        }
    }

    void OnTriggerEnter2D(Collider2D other) //checks if player hit the checkpoint. If it did startingPosition is checkpoint's position
    {
        if (other.gameObject.tag == "checkpoint")
        {
            atCheckpoint = true;
            lastCheckpointPosition = other.transform.position;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "checkpoint" && other.transform.position == lastCheckpointPosition)
        {
            startingPosition = lastCheckpointPosition; //if player has touched a checkpoint, that's the new startingPosition to respawn
        }
    }

    void Respawn() //assigs where the player has to respawn when it dies
    {
        transform.position = startingPosition;
        myBody.velocity = Vector2.zero;
        // Add any other death-related logic here, e.g. subtracting lives or showing a death animation.
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
        else if (Input.GetButtonDown("Jump") && !grounded && !doubleJump) //if we jump and we're not touching the ground, and we have alredy double jumped we can't double jump again
        {
            jump = true;
            doubleJump = true;
        }

        if (grounded)
        {
            doubleJump = false; 
        }


        //WALKING ANIMATION
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
            if (myBody.velocity.y > 0)
            {
                myBody.gravityScale = gravityScale;
            } else if (myBody.velocity.y < 0)
            {
                myBody.gravityScale = gravityFall;
            }

            //raycast is a line emerging from our player that detects if we have hit something. Is like a collision but better 
            RaycastHit2D downHit = Physics2D.Raycast(transform.position, Vector2.down, castDist);
            RaycastHit2D leftHit = Physics2D.Raycast(transform.position, Vector2.left, castDist);
            RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector2.right, castDist);

            Debug.DrawRay(transform.position, Vector2.down * castDist, Color.red); //this will show us the raycast
            Debug.DrawRay(transform.position, Vector2.left * castDist, Color.red); //this will show us the raycast to the left
            Debug.DrawRay(transform.position, Vector2.right * castDist, Color.red); //this will show us the raycast to the right


            //hitting the floor from DOWN position 
            if (downHit.collider != null && downHit.transform.CompareTag("ground")) //If I'm hitting the ground
            {
                grounded = true;
            } else
            {
                grounded = false;
            }

            //Flip direction

            if (myBody.velocity.x > 0) //we're going right 
            {
                transform.localScale = Vector3.one; //face to the right
            }
            else if (myBody.velocity.x < 0) //we're going to the left
            {
                transform.localScale = new Vector3(-1, 1, 1f); //face to the left
            }

            //WALLJUMPING

          

            //hitting the ground from LEFT position 
            if (leftHit.collider != null && leftHit.transform.CompareTag("ground")) //If I'm hitting the ground
            {
                myBody.AddForce(Vector2.one * jumpLimit, ForceMode2D.Impulse); // If I touch the wall from my left side, I impluse towards right
            }


            //hitting the ground from RIGHT position 
            if (rightHit.collider != null && rightHit.transform.CompareTag("ground")) //If I'm hitting the ground
            {
                myBody.AddForce(new Vector2(-1, 1) * jumpLimit, ForceMode2D.Impulse); //  If I touch the wall from my righ side, I impluse towards left
            }

 
            myBody.velocity = new Vector3(moveSpeed, myBody.velocity.y, 0);
     


    }
}
