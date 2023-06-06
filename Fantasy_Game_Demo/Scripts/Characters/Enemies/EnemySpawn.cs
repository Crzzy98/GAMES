using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script manages spawning for the skull enemy, 
//Skull enemies spawns in the platforming section of the stage
public class EnemySpawn : MonoBehaviour
{
    //References to various enemy prefabs
    public GameObject skull_enemyPrefab;
  
    private int checkpoints_passed = 0; //Variable tracks the amount of checkpoints the player has passed 

    //Array which holds the checkpoint positions to control enemy spawning 
    private Vector3[] checkpoint_positions;

    [SerializeField] public float enemySpawnDistance = 10f; //Variable is used to decide the distance at which to spawn enemies from player

    private Vector3 lastCheckpoint;

    //Predetermined spawn positions for enemy
    private Vector3[] skull_spawn_positions = { 
        new Vector3(541.3f,9.207467f,0),
        new Vector3(712.1f,10.1f,0),
        new Vector3(927f,8.3f,0) // Final spawn point for skull enemy
    };
  
    //Enemy tagged gameobjects are located upon game start
    void Start()
    {
        //lastCheckpoint value set to the distance player is at start of game 
        lastCheckpoint = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, 0, 0);
        //Locations of each of 3 checkpoints found and recorded 
        checkpoint_positions = new Vector3[] {
            new Vector3(GameObject.FindGameObjectWithTag("CHECK_1").transform.position.x, 0, 0),
            new Vector3(GameObject.FindGameObjectWithTag("CHECK_2").transform.position.x, 0, 0),
            new Vector3(GameObject.FindGameObjectWithTag("CHECK_3").transform.position.x, 0, 0)
        };
    }
    // Update is called once per frame
    void Update()
    {
        //spawnEnemyWave();
        spawnEnemyWave(skull_enemyPrefab,skull_spawn_positions);
    }
    
    //Method checks the player gameobjects current position
    //returns a bool value indicating whther the next wave of enemies should be spawned 
    private bool checkForCheckpoint()
    {
        if (GameObject.FindGameObjectWithTag("Player").transform.position.x > checkpoint_positions[checkpoints_passed].x - enemySpawnDistance)
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
                        if (lastCheckpoint.x < spawn_positions[i].x &&
                            spawn_positions[i].x < checkpoint_positions[checkpoints_passed].x)
                        {
                            Instantiate(enemyPrefab, spawn_positions[i],
                                enemyPrefab.transform.rotation);
                        }
                    }
                    else if (spawn_positions[i].x > lastCheckpoint.x)
                    {
                        Instantiate(enemyPrefab, spawn_positions[i],
                            enemyPrefab.transform.rotation);
                    }
                }
        }
    }
}