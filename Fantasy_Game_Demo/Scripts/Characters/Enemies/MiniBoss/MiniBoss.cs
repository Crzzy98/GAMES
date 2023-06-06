using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script contains methods for controlling the behavior of a mini boss
//Allows for the summoning of "support" minion characters and the use of multiple powers 


public class MiniBoss : MonoBehaviour
{
    Health health;
    private int currentHealth, startingHealth;
    [SerializeField] const int HALF_HEALTH = 500;
    [SerializeField] const int THIRD_OF_HEALTH = 300;
    private Camera mainCamera;

    //Variables use in minion summoning functionality
    [SerializeField]GameObject minonPrefab;  //prefab for the minion character
    [SerializeField]GameObject spawnEffectPrefab;  //prefab for spawn effect for minions
    [SerializeField] int amtOfMinons = 1;
    [SerializeField] GameObject[] specialAttackPrefabs; //Array holds gameobjects used as the prefab for various special attacks
    GameObject target;  //variable represents the target for special attacks
    [SerializeField] public int minionSpawnDistance = 10;

    void Start()
    {
        mainCamera = Camera.main;
        health = GetComponent<Health>();
        startingHealth = health.getHealth();
    }

    void Update()
    {
        checkAndSpawnMinions();
    }

    //Method checks whether the mini boss's health
    ////has reached or lowered below a certain amount and begins spawning minions
    public void checkAndSpawnMinions() 
    {
        currentHealth = health.getHealth();

        switch (currentHealth)
        {
            case HALF_HEALTH:
                spawnMinions(calculateRandomSpawnPoint());
                health.Damage(1);
                break;
            case THIRD_OF_HEALTH:
                spawnMinions(calculateRandomSpawnPoint());
                health.Damage(1);
                break;
        }
    }

    //Method insatntiates minion prefabs of predetermined amount and also a spawn effect
    public void spawnMinions(Vector2 spawnPos)
    {
        Debug.Log("Minions Spawned");
        spawnPos.y = spawnPos.y - minionSpawnDistance;

        for (int i = 0; i <= amtOfMinons; i++)
        {
            Instantiate(minonPrefab, spawnPos , Quaternion.identity);
            //Instantiate(spawnEffectPrefab, spawnPos, Quaternion.identity);
        }
    }

    //Method calculates and returns a Vector2 of a random on-screen spawn point
    //Spawns a gambeobject withthe same y position value as the current object
    public Vector2 calculateRandomSpawnPoint()
    {
        Vector2 randomSpawnPoint,
            randomWorldPosition = new Vector2();

        randomSpawnPoint = new Vector2(Random.Range(0f, 1f),
                        Random.Range(0f, 1f));
        randomWorldPosition =
           mainCamera.ViewportToWorldPoint(randomSpawnPoint);
        randomWorldPosition.y = transform.position.y;

        return randomWorldPosition;
    }

    //Method checks whether a certain amount of health has been lost
    ////and begins the routine to execute special attacks
    public void checkAndSpecialAttack()
    { 
    
    }
}
