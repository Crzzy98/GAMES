 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public int health = 100;

    private int MAX_HEALTH = 100;

    SpriteRenderer objSpriteRenderer;

    HealthDisplay health_bar;

    private void Start()
    {
        objSpriteRenderer = GetComponent<SpriteRenderer>();
        health_bar = GameObject.Find("HealthBar").GetComponent<HealthDisplay>();
    }
    //Health of gameobject with this script decreases by amount passed
    public void Damage(int amount)
    {

        if (amount < 0)
        {

            throw new System.ArgumentOutOfRangeException("Cannot process negative damage!");
        }
        else
        {
            if(gameObject.tag == "Player")
                health_bar.lowerHealth();

            this.health -= amount;
        }

        //Character dies if health reaches zero or below
        if (health <= 0)
            Die();
    }

    public void Heal(int amount)
    {

        if (amount < 0)
        {

            throw new System.ArgumentOutOfRangeException("Cannot process negative Healing!");
        }

        //Check if heal amount would make health value too great
        // greater than intended maximum health value
        bool wouldBeOverMaxHealth = health + amount > MAX_HEALTH;

        if (wouldBeOverMaxHealth)
        {
            this.health = MAX_HEALTH;
        }
        else
        {
            if (gameObject.tag == "Player")
                health_bar.raiseHealth();

            this.health += amount;
        }

        Debug.Log("Health: " + this.health + "  Amount healed: " + amount);

    }

    private void Die()
    {
      
        //StartCoroutine(FadeOut());
        Destroy(gameObject);
        if (gameObject.tag == "Player")
        {
            //display game over canvas
            GameOverScreen gameOverScreen = FindObjectOfType<GameOverScreen>();
            gameOverScreen.DisplayGameOverScreen();
        }
    }
   
    //Function returns current health value of gameobject as Integer
    public int getHealth()
    {
        return this.health;
    }
}
