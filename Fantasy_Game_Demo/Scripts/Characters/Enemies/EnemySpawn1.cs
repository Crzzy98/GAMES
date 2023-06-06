using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script manages spawning for the skeleton enemy, 
//skeleton enemies spawn near the end of the stage
public class EnemySpawn1 : MonoBehaviour
{
    //References to various enemy prefabs
    public GameObject skeleton_enemyPrefab;

    private int checkpoints_passed = 0; //Variable tracks the amount of checkpoints the player has passed 

    //Array which holds the checkpoint positions to control enemy spawning 
    private Vector3[] checkpoint_positions1;

    [SerializeField] public float enemySpawnDistance = 10f; //Variable is used to decide the distance at which to spawn enemies from player

    private Vector3 lastCheckpoint1;

    //Predetermined spawn positions for enemy(Decide on positions after level building)
    private Vector3[] skeleton_spawn_positions = {
        new Vector3(1021.3f,.8f,0),
        new Vector3(29.3f,2f,0),
        new Vector3(1051.7f,1.5f,0) // Final spawn point for skeleton enemy
    };

    //Enemy tagged gameobjects are located upon game start
    void Start()
    {
        //lastCheckpoint value set to the distance player is at start of game 
        lastCheckpoint1 = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, 0, 0);
        //Locations of each of 3 checkpoints found and recorded 
        checkpoint_positions1 = new Vector3[] {
            new Vector3(GameObject.FindGameObjectWithTag("CHECK_1").transform.position.x, 0, 0),
            new Vector3(GameObject.FindGameObjectWithTag("CHECK_2").transform.position.x, 0, 0),
            new Vector3(GameObject.FindGameObjectWithTag("CHECK_3").transform.position.x, 0, 0)
        };
    }
    // Update is called once per frame
    void Update()
    {
        //spawnEnemyWave();
        spawnEnemyWave(skeleton_enemyPrefab, skeleton_spawn_positions);
    }
    
    //Method checks the player gameobjects current position
    //returns a bool value indicating whther the next wave of enemies should be spawned 
    private bool checkForCheckpoint()
    {
        if (GameObject.FindGameObjectWithTag("Player").transform.position.x > checkpoint_positions1[checkpoints_passed].x )
        {
            lastCheckpoint1.x = checkpoint_positions1[checkpoints_passed].x;
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
                        if (lastCheckpoint1.x < spawn_positions[i].x &&
                            spawn_positions[i].x < checkpoint_positions1[checkpoints_passed].x)
                        {
                            Instantiate(enemyPrefab, spawn_positions[i],
                                enemyPrefab.transform.rotation);
                        }
                    }
                    else if (spawn_positions[i].x > lastCheckpoint1.x)
                    {
                        Instantiate(enemyPrefab, spawn_positions[i],
                            enemyPrefab.transform.rotation);
                    }
                }
        }
    }
}