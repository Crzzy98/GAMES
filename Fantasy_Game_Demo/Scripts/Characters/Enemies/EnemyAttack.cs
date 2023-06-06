using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    public GameObject attack;
    public GameObject pacing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(pacing){ attack.SetActive(false);}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Attack animation activated when player collides
        if (collision.gameObject.tag == "Player")
        {
            pacing.SetActive(false);
            attack.SetActive(true);
        }
        else 
        {
            attack.SetActive(false);
            pacing.SetActive(true);
        }
    }
}
