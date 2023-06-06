using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script contains functions which control pickup behaviors
public class PickupBehavior : MonoBehaviour
{
    public Vector3 bobbingStartPos;
    bool isHeadedUp, isHeadedDown;
    [SerializeField] private float bobbing_speed = 1f;    //Variable representing the speed at which the pcikup move while bobbing
    [SerializeField] float bobbingHeight = 1f;
    [SerializeField] int amountToHeal = 20;

    void Start()
    {
        bobbingStartPos = new Vector3(0, gameObject.transform.position.y, 0);
    }

    void Update()
    {
        applyBobbingMotion(gameObject, bobbingStartPos, bobbingHeight, bobbing_speed);
    }
   
    //Method controls the bobbing physics of pickup items
    public void applyBobbingMotion(GameObject bobbingObj, Vector3 startPos, float bobHeight, float bobSpeed)
    {
        bool isBobbing = false;
        //Check if enemy is performing bob motion/ not at bob height or start position 
        //if enemy is at the start postion, the bob motion will begin
        if (isHeadedUp)
        {
            bobbingObj.transform.Translate(Vector3.up * Time.deltaTime * bobbing_speed);
        }
        else if (isHeadedDown)
        {
            bobbingObj.transform.Translate(Vector3.down * Time.deltaTime * bobbing_speed);
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

    //Method allows current gameobject to heal the player upon collision
    public void healThePlayer(int amount, GameObject player)
    {
        player.GetComponent<Health>().Heal(amount);
    }
    //Method increases the player strength attributes 
    public void powerUpPlayer()
    { 
    
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Player is healed when colliding with health pickup and game object is deactivated
        if (collision.gameObject.tag == "Health_Pickup")
        {
            healThePlayer(amountToHeal, collision.gameObject);
            gameObject.SetActive(false);
        }
    }
}
