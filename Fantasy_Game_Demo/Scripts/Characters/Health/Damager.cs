using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] public GameObject damageTarget;
    public int damage = 20;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Player hit");
            Health health = collision.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.Damage(damage);
            }
        }
    }
}
