using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script manages spawning for the slime enemy, 
//Slime enemies spawn near the beginning of the stage
public class EnemySpawn2 : MonoBehaviour
{
    //References to various enemy prefabs
    public GameObject slime_enemyPrefab;

    private int checkpoints_passed = 0; //Variable tracks the amount of checkpoints the player has passed 

    //Array which holds the checkpoint positions to control enemy spawning 
    private Vector3[] checkpoint_positions2;

    [SerializeField] public float enemySpawnDistance = 10f; //Variable is used to decide the distance at which to spawn enemies from player

    private Vector3 lastCheckpoint2;

    //Predetermined spawn positions for enemies(Decide on positions after level building)
    private Vector3[] slime_spawn_positions = {
        new Vector3(26f,-2f,0),
        new Vector3(49f,-1f,0),
        new Vector3(65f,-2f,0),
        new Vector3(89f,0f,0),
        new Vector3(137f,-1f,0),
        new Vector3(146f,-1f,0),
        new Vector3(151f,-1f,0),
        new Vector3(159f,-1f,0),
        new Vector3(590f,4f,0)
    };

    //Enemy tagged gameobjects are located upon game start
    void Start()
    {
        //lastCheckpoint value set to the distance player is at start of game 
        lastCheckpoint2 = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, 0, 0);
        //Locations of each of 3 checkpoints found and recorded 
        checkpoint_positions2 = new Vector3[] {
            new Vector3(GameObject.FindGameObjectWithTag("CHECK_1").transform.position.x, 0, 0),
            new Vector3(GameObject.FindGameObjectWithTag("CHECK_2").transform.position.x, 0, 0),
            new Vector3(GameObject.FindGameObjectWithTag("CHECK_3").transform.position.x, 0, 0)
        };
    }
    // Update is called once per frame
    void Update()
    {
        //spawnEnemyWave();
        spawnEnemyWave(slime_enemyPrefab, slime_spawn_positions);
    }
    
    //Method checks the player gameobjects current position
    //returns a bool value indicating whther the next wave of enemies should be spawned 
    private bool checkForCheckpoint()
    {
        if (GameObject.FindGameObjectWithTag("Player").transform.position.x > checkpoint_positions2[checkpoints_passed].x - enemySpawnDistance)
        {
            checkpoints_passed++;
            return true;
        }
        else
            return false;
    }

    //Method spawns enemies based on prefab parameter and distance to/between checkpoints
    private void spawnEnemyWave(GameObject enemyPrefab, Vector3[] spawn_positions)
    {
        // Check if player has passed activation/check point
        if (checkForCheckpoint())
        {
                // Activate each enemy in array using a for loop
                for (int i = 0; i < spawn_positions.Length; i++)
                {
                    if (checkpoints_passed < 3)
                    {
                        //Enemies are instantiated
                        //enemies are spawned based on spawn point being between lastCheckpoint and the next checkpoint
                        if (lastCheckpoint2.x < spawn_positions[i].x &&
                            spawn_positions[i].x < checkpoint_positions2[checkpoints_passed].x)
                        {
                            Instantiate(enemyPrefab, spawn_positions[i],
                                enemyPrefab.transform.rotation);
                        }
                    }
                    else if (spawn_positions[i].x > lastCheckpoint2.x)
                    {
                        Instantiate(enemyPrefab, spawn_positions[i],
                            enemyPrefab.transform.rotation);
                    }
                }
        }
    }
}