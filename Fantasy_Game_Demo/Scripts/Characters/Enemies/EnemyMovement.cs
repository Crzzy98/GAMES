using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private GameObject player;  //Variable referencing the player game object
    GameObject enemy;   //Variable used to refernce enemy object when appplying "move to player" behavior                        
    public GameObject[] enemies; // Gameobj array declared to hold spawned enemies
    [SerializeField] private float enemy_speed = 2f;    //Variable representing the speed at which the enemy character will move toward the player

    //Variables used in processing patrolling/pacing enemy behavior
    public Transform current_enemy;
    public Vector3 patrolStartPos;
    bool pacingLeft, pacingRight;   //Variables indicating whether enemy is pacing in either direction
    [SerializeField] float enemyPatrolDistance = 5; //Variable used set the distance that a "pacing" enemy will walk before turning

    //Variables used in bobbing animation behavior 
    bool isHeadedUp, isHeadedDown;
    [SerializeField] float bobbingHeight = 10;
    public Vector3 bobbingStartPos;
    private int flips = 0;

    void Start()
    {
        player = GameObject.Find("Player");
        //Motions applied to enemies 
            patrolStartPos = new Vector3(gameObject.transform.position.x, 0, 0);
            bobbingStartPos = new Vector3(0, gameObject.transform.position.y, 0);

        //Generate a random number between 0 and 10
        int randomNum = Random.Range(0, 11);

        //Check if random number is even
        if ((randomNum * Time.deltaTime) % 2 == 0)
        {
            //Subtract the patrol distance  from the x-axis of the current game object's position
            transform.Translate(new Vector3(enemyPatrolDistance, 0f, 0f));
        }
    }

     //Each frame the current enemies in game will move toward player
    //Enemies should either spawn due to proximity
    //or existing enemies in game world only move when in proximty to player
    void Update()
    {
        //Method call causes enemy characters to move towwards the player 
        //moveToPlayer();

        //Enemy type checked by tag to determine script behavior below

        //Floting skull enemy behavior defined here
        if (tag == "Skull_Enemy") 
        {
            applyBobbingMotion(gameObject, bobbingStartPos, bobbingHeight,enemy_speed);
            applyBackAndForthMotion(patrolStartPos, enemyPatrolDistance, gameObject);
        }

        if (tag == "Skeleton_Enemy")
            applyBackAndForthMotion(patrolStartPos, enemyPatrolDistance, gameObject);

        if (tag == "Slime_Enemy" )
            applyBackAndForthMotion(patrolStartPos, enemyPatrolDistance, gameObject);
    }
 
    //Method will cause the gameObject passed as the third argument to "pace" back and forth
    //The two float argument repesent the starting position and the distance the enemy will travel while pacing respectively
    //The method utilizes two bool class variables to detect direction the enemy is heading { var1: pacingLeft var2:pacingRight}
    private void applyBackAndForthMotion(Vector3 startPos,float patrolDistance, GameObject patrollingEnemy) 
    {
        bool isPacing = false;
        bool needsToFlip = false;

        //Check if enemy is pacing left and continue translation
        if (pacingLeft)
        {
            patrollingEnemy.transform.Translate(Vector3.left * Time.deltaTime * enemy_speed);
        }
        else if (pacingRight)
        {
            patrollingEnemy.transform.Translate(Vector3.right * Time.deltaTime * enemy_speed);
        }
        //Check if enemy is between patrol bounds/ is currently "pacing"
        if (patrollingEnemy.transform.position.x < startPos.x &&
            patrollingEnemy.transform.position.x > startPos.x - patrolDistance)
        {
            isPacing = true;
        }
        else
        { 
            isPacing = false;
            pacingLeft = false;
            pacingRight = false;

        }

        //Check direction of enemy and 
        //Check if enemy has stopped pacing
        if (!isPacing)
            if (patrollingEnemy.transform.position.x >= startPos.x)
            {
               
                //For enemy to start pacing towards the left
                patrollingEnemy.transform.Translate(Vector3.left * Time.deltaTime * enemy_speed);
                pacingLeft = true;

                if (flips > 0)
                    needsToFlip = true;

            }
            else if (patrollingEnemy.transform.position.x <= startPos.x - patrolDistance)
            {
                //For enemy to start pacing towards the right
                patrollingEnemy.transform.Translate(Vector3.right * Time.deltaTime * enemy_speed);
                pacingRight = true;

                flips++;
                needsToFlip = true;
            }

        if (needsToFlip)
        {
            patrollingEnemy.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            needsToFlip = false;
        }

    }
    //Method will make the game object "bobbingObj" bob up and down based on y axis translations
    //Mass of game object may need to be altered to change bob height
    public void applyBobbingMotion(GameObject bobbingObj, Vector3 startPos, float bobHeight,float bobSpeed)
    {
        bool isBobbing = false;
        //Check if enemy is performing bob motion/ not at bob height or start position 
        //if enemy is at the start postion, the bob motion will begin
        if (isHeadedUp)
        {
            bobbingObj.transform.Translate(Vector3.up * Time.deltaTime * enemy_speed);
        }
        else if (isHeadedDown)
        {
            bobbingObj.transform.Translate(Vector3.down * Time.deltaTime * enemy_speed);
        }
        //Check if enemy is between patrol bounds/ is currently "bobbing"
        if (bobbingObj.transform.position.y > startPos.y &&
            bobbingObj.transform.position.y < startPos.y + bobHeight)
        {
            isBobbing = true;
        }
        else
        {
            isBobbing = false;
            isHeadedUp = false;
            isHeadedDown = false;
        }
        if (!isBobbing)
            if (bobbingObj.transform.position.y <= startPos.y)
            {
                bobbingObj.transform.Translate(Vector3.up * Time.deltaTime * bobSpeed);
                isHeadedUp = true;
            }
            else if (bobbingObj.transform.position.y >= startPos.y + bobHeight)
            {
                bobbingObj.transform.Translate(Vector3.down * Time.deltaTime * bobSpeed);
                isHeadedDown = true;
            }
    }
    private void moveToPlayer() {
        enemies = GameObject.FindGameObjectsWithTag("Follow_Player");
        foreach (GameObject current_enemy in enemies)
            if (player.transform.position.x < current_enemy.transform.position.x)// move enemy left 
            {
                current_enemy.transform.Translate(Vector3.left * Time.deltaTime * enemy_speed);
            }
            else if (player.transform.position.x > current_enemy.transform.position.x)// move enemy right
            {
                current_enemy.transform.Translate(Vector3.right * Time.deltaTime * enemy_speed);
            }
    }
    
}
