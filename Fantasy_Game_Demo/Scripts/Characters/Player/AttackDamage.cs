using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script manages damage for attacks that exist as prefabs
//The attack prefab is ddestroyed after 
public class AttackDamage : MonoBehaviour
{
    public int damage = 20;
    public float speed = 20f;
    public float lifetime = 1.5f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameObject.tag == "Projectile")
        {
            transform.position += transform.right * Time.deltaTime * speed;
            Destroy(gameObject, lifetime);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Skeleton_Enemy"
            || collision.gameObject.tag == "Skull_Enemy"
            || collision.gameObject.tag == "Slime_Enemy")
        {
            try 
            {
                Health health = collision.gameObject.GetComponent<Health>();
                if (health != null)
                {
                    health.Damage(damage);
                }
            }
            catch (System.Exception e) 
            { 
                Debug.Log(e.StackTrace); } 
            finally
            {
                //Deactivate and play animation for landed hits 
                gameObject.SetActive(false);
            }
        }
    }
}
