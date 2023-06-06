using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    private GameObject attackArea = default;

    private bool attacking = false;

    private float timeToAttack = 0.35f;
    private float timer = 0f;

    //Varables used in sound effect behavior
    [SerializeField] public AudioClip soundFile; // Assign the sound file in the Inspector
    private AudioSource audioSource;
    void Start()
    {
        //gets the first child of this gameobject
        attackArea = transform.GetChild(0).gameObject;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Primary attack executed if right mouse button is pressed
        if (Input.GetKey(KeyCode.Mouse1))
        {
            primaryAttack();
            playSoundEffect();
        }

        //Code ensures attack stays active for certain duration of time
        if (attacking)
        {
            timer += Time.deltaTime;    //variable holds incrementing value based on real-time

            if (timer >= timeToAttack)  //checks if attack time duration has beeen reached
            {
                timer = 0;  //timer reset
                attacking = false;  //attack stopped
                attackArea.SetActive(attacking);
            }
        }
    }

    private void primaryAttack()
    {
        attacking = true;
        attackArea.SetActive(attacking);
        
    }
    public void playSoundEffect()
    {
        audioSource.PlayOneShot(soundFile);
    }
}
