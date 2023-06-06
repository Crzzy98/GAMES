using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    //Variables below used in platform motion processing
    [SerializeField] public float platformSpeed = 2;    //Variable reperesenting the desired speed of moving platforms
    private bool 
        movingRight = false,
        movingLeft = false; //Variables used to indiceate the current direction of a moving platform
    [SerializeField] public float platformMoveDistance = 5;
    private Vector3 movingPlatformStartPos;
    GameObject movingPlatform;

    //Variables below are used to control object shake parameters
    private bool
        shakingLeft = false,
        shakingRight = false;
    [SerializeField] public float shakeDistance = 1;
    [SerializeField] public float shakesToDo = 8;  //Variable representing the amount of side-to-side shakes an object will do before disappearing
    private Vector3 shakingPlatformStartPos;
    [SerializeField] public float shakeSpeed = 3.5f;
    GameObject shakingPlatform;
    private bool shakeStarted;

    SpriteRenderer objSpriteRenderer;
    //Platform type is organized by tag: Moving_Platform, Shaking_Platform
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.tag == "Moving_Platform")
        {
            movingPlatform = gameObject;
            movingPlatformStartPos = new Vector3(movingPlatform.transform.position.x, 0, 0);
        }

        //Shaking platform gameobjects are collected in array
        if (gameObject.tag == "Shaking_Platform")
        {
            shakingPlatform = gameObject;
            shakingPlatformStartPos = new Vector3(shakingPlatform.transform.position.x, 0, 0);
        }
        //Debug.Log("STARTPOS:" + shakingPlatform.transform.position.x);

        objSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Back and forth motion is applied to platform with the appropriate tag
        if (GameObject.FindGameObjectsWithTag("Moving_Platform").Length != 0)
            applyBackAndForthMotion(movingPlatformStartPos, platformMoveDistance, movingPlatform);
    }
    private void LateUpdate()
    {
        //Platforms will start shaking upon player collision (see OnCollisionEnter2D)
        if (shakeStarted)
            shakeAndDisappear(shakingPlatform, shakingPlatformStartPos, shakeDistance);
    }
    private void applyBackAndForthMotion(Vector3 startPos, float translateDistance, GameObject movingObj)
    {
        bool isMoving = false;
        //Check if enemy is pacing left and continue translation
        if (movingLeft)
        {
            movingObj.transform.Translate(Vector3.left * Time.deltaTime * platformSpeed);
        }
        else if (movingRight)
        {
            movingObj.transform.Translate(Vector3.right * Time.deltaTime * platformSpeed);
        }
        //Check if enemy is between patrol bounds, is currently "pacing"
        if (movingObj.transform.position.x < startPos.x &&
            movingObj.transform.position.x > startPos.x - translateDistance)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
            movingLeft = false;
            movingRight = false;
        }
        //Translate statement must execute every update 
        //Check direction of enemy and 
        //Check if enemy has stopped pacing
        if (!isMoving)
            if (movingObj.transform.position.x >= startPos.x)
            {
                //For enemy to start pacing towards the left
                movingObj.transform.Translate(Vector3.left * Time.deltaTime * platformSpeed);
                movingLeft = true;
            }
            else if (movingObj.transform.position.x <= startPos.x - translateDistance)
            {
                //For enemy to start pacing towards the right
                movingObj.transform.Translate(Vector3.right * Time.deltaTime * platformSpeed);
                movingRight = true;
            }
    }

    //Object will vibrate/shake side-to-side and then disappear
    private void shakeAndDisappear(GameObject shakingObj,Vector3 startPos, float shakeDistance)
    {
        bool isMoving = false;
        //Check if enemy is shaking towards the left and continue translation
        if (shakingLeft)
        {
            shakingObj.transform.Translate(Vector3.left * Time.deltaTime * shakeSpeed);
        }
        else if (shakingRight)
        {
            shakingObj.transform.Translate(Vector3.right * Time.deltaTime * shakeSpeed);
        }
        //Check if enemy is between shake bounds, is currently "shaking"
        if (shakingObj.transform.position.x < startPos.x &&
            shakingObj.transform.position.x > startPos.x - shakeDistance)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
            shakingLeft = false;
            shakingRight = false;
        }
        if (!isMoving)
            if (shakingObj.transform.position.x >= startPos.x)
            {
                //For enemy to start shaking towards the left
                shakingObj.transform.Translate(Vector3.left * Time.deltaTime * shakeSpeed);
                shakingLeft = true;
                shakesToDo--;

            }
            else if (shakingObj.transform.position.x <= startPos.x - shakeDistance)
            {
                //For enemy to start shaking towards the right
                shakingObj.transform.Translate(Vector3.right * Time.deltaTime * shakeSpeed);
                shakingRight = true;
            }
        if (shakesToDo == 5) //The object disappears
        {
            //Fades and is destroyed
            StartCoroutine(FadeOut());
        }
    }
    //Method causes GameObject to fade and then destroys the object
    IEnumerator FadeOut()
    {
        for (float i = 1f; i >= -0.05f; i -= .05f) 
        {
            Color objectColor = objSpriteRenderer.material.color;
            objectColor.a = i;
            objSpriteRenderer.material.color = objectColor;
            yield return new WaitForSeconds(0.05f);
        }
        GameObject.Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (gameObject.tag == "Shaking_Platform" && 
                PlayerMovement.grounded == true)
            {
                shakeStarted = true;
            }
        }
    }
}
