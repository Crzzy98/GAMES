using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Fields used to access and manipulate player character 
    private Rigidbody2D playerRb;
    private SpriteRenderer sprite;
    private BoxCollider2D thisColllider;

    private float direction;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] private float jumpSpeed = 11.5f;
    public bool gameOver = false;

    //Jump control variables
    [SerializeField] public float bounceDistance = 10.0f;
    [SerializeField] public float bounceHeight = 10.0f;

    public float fallMultiplier = 2.5f;
    public float lowFallMultiplier = 2f;
    [SerializeField] public LayerMask groundTerrain;
    public static bool grounded;

    GameOverScreen gameOverScreen;

    private float previousVelocityX;
    private bool
        facingRight = true, 
        facingLeft = false;    //Value indicating whether the player is facing right or not

    //Variables used for camera freeze behavior
    [SerializeField] GameObject myCamera;

    //Create array of soundfiles, write switch statement in sound method to choose based on action jump, land, etc****************
    [SerializeField] public AudioClip[] soundFiles; // Assign the sound file in the Inspector
    private AudioSource audioSource;

    enum playerSoundIndexes
    { 
    JUMP_SOUND_INDEX 
    };

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        thisColllider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        gameOverScreen = FindObjectOfType<GameOverScreen>();
        previousVelocityX = playerRb.velocity.x;
    }
    // Update is called once per frame
    void Update()
    {
        grounded = isOnGround();

        //X axis input is received
        direction = Input.GetAxisRaw("Horizontal");

        //player velocity is changed based on horizontal velocity (Player side-to-side movement)
        playerRb.velocity = new Vector2(direction * moveSpeed, playerRb.velocity.y);

        //jump button press is detected and y velocity is increased
        if (grounded && Input.GetButtonDown("Jump")  )
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpSpeed);
            playSoundEffect("Jump");
        }

        //Check if player is falling via y velocity
        if (playerRb.velocity.y < 0)
        {
            //Downward velocity is increased in proportion to the value of fallmultiplier variable
            playerRb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (playerRb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            playerRb.velocity += Vector2.up * Physics2D.gravity.y * (lowFallMultiplier - 1) * Time.deltaTime;
        }
       
        //Player animations are updated
        updateAnimation();

        previousVelocityX = playerRb.velocity.x;

        //Check if player has reached the end of game
        checkForGameOver();

    }
    //Method returns a boolean value indicating whether player is on ground or not
    private bool isOnGround()
    {
        //This statement creates an invisible box translated down .1 units from the origin of the player
        //returns true if it coilides with the layer assigned to the variable "groundTerrain"
        return Physics2D.BoxCast(thisColllider.bounds.center, thisColllider.bounds.size, 0f,
            Vector2.down, .1f, groundTerrain);
    }
    //Method updates the animations for the character based on its axis velocity
  //Check that player rotation is at 180 for facingright check
    private void updateAnimation()
    {
        if (!facingLeft && 
            Mathf.Sign(previousVelocityX) != Mathf.Sign(playerRb.velocity.x)
            && playerRb.velocity.x < 0)
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            currentRotation.y += 180f;
            transform.rotation = Quaternion.Euler(currentRotation);
            Debug.Log("Previous velocity:" + previousVelocityX);
            facingRight = false;
            facingLeft = true;
        }
        else if (!facingRight 
            && previousVelocityX == 0
            && playerRb.velocity.x > 0)
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            currentRotation.y -= 180f;
            transform.rotation = Quaternion.Euler(currentRotation);
            facingRight = true;
            facingLeft = false;
        }
    }

    //Method detects collisons with other objects
    //Used to detect enemy - player collisons
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")    //player collides with enemy tagged game object
        {
            //Player should "bounce" away from enemy
            if (collision.gameObject.transform.position.x < transform.position.x)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    collision.gameObject.transform.position + Vector3.right * bounceDistance,
                    bounceHeight * Time.deltaTime);

                transform.position = Vector3.MoveTowards(transform.position,
                   collision.gameObject.transform.position + Vector3.up * bounceDistance,
                   bounceHeight * Time.deltaTime);
            }
            else if (collision.gameObject.transform.position.x > transform.position.x)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    collision.gameObject.transform.position + Vector3.left * bounceDistance,
                    bounceHeight * Time.deltaTime);

                transform.position = Vector3.MoveTowards(transform.position,
                  collision.gameObject.transform.position + Vector3.up * bounceDistance,
                  bounceHeight * Time.deltaTime);
            }
        }
        else if (collision.gameObject.tag == "HealthCross")
        {

            //player gains 20 percent health on contact with health pickup
            //health_bar.raiseHealth();

            //Destroy(collision.gameObject);  //Health cross is destroyed upon contact with player

            //health.Heal(healAmt);
        }
    }
    //Method checks if the player has reached or passed the game over checkpoint at the end of the game
    private void checkForGameOver()
    {
        if (gameObject.transform.position.x >=
            GameObject.FindGameObjectWithTag("Game Over").transform.position.x ||
            gameObject.transform.position.y <= 
            GameObject.FindGameObjectWithTag("Game_Over_Floor").transform.position.y)
        {
            gameOverScreen.DisplayGameOverScreen();
        }
    }

    //Method plays sound corresponding to the current player action
    private void playSoundEffect(string action) 
    {
        //Switch matches the name of the player action with its index placement in the soundfiles array
        switch (action) {
            case "Jump":
                audioSource.PlayOneShot(soundFiles[(int)playerSoundIndexes.JUMP_SOUND_INDEX]);
                break;

        }

    }
}
